using System.Globalization;
using Client;
using Core;
using Core.Auth;
using Core.Database;
using Core.Web;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using Service.ProductInstallDataCollect;

namespace Web.Controllers;

[NoAuth]
public class ProductInstallDataCollectController(
    Db db,
    ProductInstallDataCollectService productInstallDataCollectService) : Controller
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
                new DateTimeFormatInfo(),
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

        long productId = clientKey.ProductId;
        long collegeId = clientKey.CollegeId;

        var productInstallMonitor = db.ProductInstallMonitor.AsTracking() // 后面可能有修改
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

            var provinceType = college.Province;

            productInstallMonitor = new ProductInstallMonitorEntity {
                ProductId = productId,
                CollegeId = collegeId,
                ProvinceType = provinceType,
                CpuSerialNo = param.CpuSerialNo,
                NetMacAddress = param.NetMacAddress,
                TaskCount = 0, // 这两个数据暂时这样了
                TeacherCount = 0, // 这两个数据暂时就这样了
            };

            db.ProductInstallMonitor.Add(productInstallMonitor);
        }
        else
        {
            // 如果这两个数据跟原来存储的数据一样，是不会生成 update 语句进行实际更改的
            productInstallMonitor.CpuSerialNo = param.CpuSerialNo;
            productInstallMonitor.NetMacAddress = param.NetMacAddress;
        }

        // 先把主表保存一下
        db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }
}