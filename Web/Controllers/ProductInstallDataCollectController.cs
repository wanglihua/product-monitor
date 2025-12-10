using System.Globalization;
using Client;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using Service;
using Service.ProductInstallDataCollect;

namespace Web.Controllers;

[NoAuth]
public class ProductInstallDataCollectController(
    Db db,
    ProductInstallDataCollectService productInstallDataCollectService,
    IpRegionService ipRegionService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/product/install/data/collect")]
    [Transactional(true)]
    public IActionResult Index([FromBody] ProductInstallDataCollectParam param)
    {
        // 先做校验处理，校验失败的，就不做后续的检查和日志记录了
        if (productInstallDataCollectService.VerifySign(param) == false)
        {
            // 记录到日志
            string message = "ProductInstallDataCollectParam 数据签名校验失败: " + JsonConvert.SerializeObject(param);
            log.Error(message);
            throw new Exception(message);
        }

        if (string.IsNullOrWhiteSpace(param.Key) == true)
        {
            // 记录到日志
            throw new Exception("产品安装数据监测数据，客户端密钥不能为空！");
        }

        if (DateTime.TryParseExact(
                param.UseTime,
                ProductInstallDataCollectParam.DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var useTime) == false)
        {
            // 记录到日志
            throw new Exception("产品安装数据监测数据，使用时间为空或格式不正确！");
        }

        if (param.AccountType != ProductInstallDataCollectParam.AccountTypeAdmin &&
            param.AccountType != ProductInstallDataCollectParam.AccountTypeTeacher &&
            param.AccountType != ProductInstallDataCollectParam.AccountTypeStudent)
        {
            // 记录到日志
            throw new Exception($"产品安装数据监测数据，无效的账号类型#{param.AccountType}");
        }

        if (string.IsNullOrWhiteSpace(param.LoginUser) == true)
        {
            throw new Exception("产品安装数据监测数据，使用账号不能为空！");
        }

        var accountType = AccountType.None;
        if (param.AccountType == ProductInstallDataCollectParam.AccountTypeAdmin)
        {
            accountType = AccountType.Admin;
        }

        if (param.AccountType == ProductInstallDataCollectParam.AccountTypeTeacher)
        {
            accountType = AccountType.Teacher;
        }

        if (param.AccountType == ProductInstallDataCollectParam.AccountTypeStudent)
        {
            accountType = AccountType.Student;
        }

        if (string.IsNullOrWhiteSpace(param.LoginUser) == true)
        {
            // 记录到日志
            throw new Exception("产品安装数据监测数据，登录账户不能为空！");
        }

        string loginUser = param.LoginUser;

        // CpuSerialNo NetMacAddress 暂时不检查
        string cpuSerialNo = param.CpuSerialNo ?? ""; // 参数不检查，所以加上 ?? ""
        string netMacAddress = param.NetMacAddress ?? ""; // 参数不检查，所以加上 ?? ""

        // ip 地址
        string ipAddress = WebUtil.GetClientIp(Request);

        // 使用省份
        string userProvinceStr = ipRegionService.GetIpAddressProvince(ipAddress);
        var useProvince = ProvinceType.GetByName(userProvinceStr);

        string key = param.Key;
        var clientKey = db.ClientKey.AsNoTracking()
            .Where(e => e.ClientKey == key)
            .Take(1)
            .SingleOrDefault();
        if (clientKey is null)
        {
            // 记录到日志
            throw new Exception($"指定的客户端密钥#{key}不存在！");
        }

        if (clientKey.KeyType != ClientKeyType.Install)
        {
            // 记录到日志
            throw new Exception($"指定的客户端密钥#{key}不是产品安装类型！");
        }

        long productId = clientKey.ProductId;
        long collegeId = clientKey.CollegeId;

        var productInstallMonitor = db.ProductInstallMonitor.AsTracking() // 后面可能对这个数据有修改
            .Where(e => e.ProductId == productId && e.CollegeId == collegeId)
            .Take(1)
            .SingleOrDefault();

        if (productInstallMonitor is null)
        {
            var college = db.College.Find(collegeId);
            if (college is null)
            {
                // 记录到日志
                throw new Exception($"指定的院校#{collegeId}不存在！");
            }

            var collegeProvinceType = college.Province;

            productInstallMonitor = new ProductInstallMonitorEntity {
                ProductId = productId,
                CollegeId = collegeId,
                ProvinceType = collegeProvinceType,
                CpuSerialNo = cpuSerialNo,
                NetMacAddress = netMacAddress,
                TaskCount = 0, // 这两个数据暂时这样了
                TeacherCount = 0, // 这两个数据暂时就这样了
            };

            db.ProductInstallMonitor.Add(productInstallMonitor);
        }
        else
        {
            // 如果这两个数据跟原来存储的数据一样，是不会生成 update 语句进行实际更改的
            productInstallMonitor.CpuSerialNo = cpuSerialNo;
            productInstallMonitor.NetMacAddress = netMacAddress;
        }

        // 先把主表保存一下
        db.SaveChanges();

        long productInstallMonitorId = productInstallMonitor.Id;

        var productInstallMonitorEntry = new ProductInstallMonitorEntryEntity {
            ProductMonitorId = productInstallMonitorId,
            LoginUser = loginUser,
            AccountType = accountType,
            IpAddress = ipAddress,
            Province = userProvinceStr,
            ProvinceType = useProvince,
            CpuSerialNo = cpuSerialNo,
            NetMacAddress = netMacAddress,
            UseTime = useTime,
        };

        db.ProductInstallMonitorEntry.Add(productInstallMonitorEntry);
        db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }
}