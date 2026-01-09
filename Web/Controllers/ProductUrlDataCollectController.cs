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
using Service.ProductUrlDataCollect;

namespace Web.Controllers;

[NoAuth]
public class ProductUrlDataCollectController(
    Db db,
    IpRegionService ipRegionService,
    ProductUrlDataCollectService productUrlDataCollectService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/product/url/data/collect")]
    [Transactional(true)]
    public IActionResult Index([FromBody] ProductUrlDataCollectParam param)
    {
        if (productUrlDataCollectService.VerifySign(param) == false)
        {
            // 记录到日志
            string message = "ProductUrlDataCollectParam 数据签名校验失败: " + JsonConvert.SerializeObject(param);
            log.Error(message);
            throw new Exception(message);
        }

        if (string.IsNullOrWhiteSpace(param.Key) == true)
        {
            // 记录到日志
            throw new Exception("网址数据监测数据，客户端密钥不能为空！");
        }

        if (DateTime.TryParseExact(
                param.UseTime,
                ProductUrlDataCollectParam.DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var useTime) == false)
        {
            // 记录到日志
            throw new Exception("网站数据监测数据，使用时间为空或格式不正确！");
        }

        if (param.AccountType != ProductUrlDataCollectParam.AccountTypeAdmin &&
            param.AccountType != ProductUrlDataCollectParam.AccountTypeTeacher &&
            param.AccountType != ProductUrlDataCollectParam.AccountTypeStudent)
        {
            // 记录到日志
            throw new Exception($"网站数据监测数据，无效的账号类型#{param.AccountType}");
        }

        if (string.IsNullOrWhiteSpace(param.LoginUser) == true)
        {
            throw new Exception("网站数据监测数据，使用账号不能为空！");
        }

        var accountType = AccountType.None;
        if (param.AccountType == ProductUrlDataCollectParam.AccountTypeAdmin)
        {
            accountType = AccountType.Admin;
        }

        if (param.AccountType == ProductUrlDataCollectParam.AccountTypeTeacher)
        {
            accountType = AccountType.Teacher;
        }

        if (param.AccountType == ProductUrlDataCollectParam.AccountTypeStudent)
        {
            accountType = AccountType.Student;
        }

        if (string.IsNullOrWhiteSpace(param.LoginUser) == true)
        {
            // 记录到日志
            throw new Exception("网站数据监测数据，登录账户不能为空！");
        }

        string loginUser = param.LoginUser;

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

        if (clientKey.KeyType != ClientKeyType.UrlEval
            && clientKey.KeyType != ClientKeyType.UrlRent
            && clientKey.KeyType != ClientKeyType.UrlSite)
        {
            // 记录到日志
            throw new Exception($"指定的客户端密钥#{key}不是网站类型！");
        }

        long productId = clientKey.ProductId;
        long collegeId = clientKey.CollegeId;

        var productUrlMonitor = db.ProductUrlMonitor.AsTracking() // 后面可能对这个数据有修改
            .Where(e => e.ProductId == productId && e.CollegeId == collegeId)
            .Take(1)
            .SingleOrDefault();

        if (productUrlMonitor is null)
        {
            var college = db.College.Find(collegeId);
            if (college is null)
            {
                // 记录到日志
                throw new Exception($"指定的院校#{collegeId}不存在！");
            }

            var productUrlType = ProductUrlType.None;
            if (clientKey.KeyType == ClientKeyType.UrlEval)
            {
                productUrlType = ProductUrlType.Eval;
            }

            if (clientKey.KeyType == ClientKeyType.UrlRent)
            {
                productUrlType = ProductUrlType.Rent;
            }

            if (clientKey.KeyType == ClientKeyType.UrlSite)
            {
                productUrlType = ProductUrlType.Site;
            }


            productUrlMonitor = new ProductUrlMonitorEntity {
                ProductId = productId,
                CollegeId = collegeId,
                ProductUrlType = productUrlType,
                TaskCount = 0, // 这两个数据暂时这样了
                TeacherCount = 0, // 这两个数据暂时就这样了
            };

            db.ProductUrlMonitor.Add(productUrlMonitor);
        }

        // 先把主表保存一下
        db.SaveChanges();

        long productUrlMonitorId = productUrlMonitor.Id;

        var productUrlMonitorEntry = new ProductUrlMonitorEntryEntity {
            ProductUrlMonitorId = productUrlMonitorId,
            AccountType = accountType,
            LoginUser = loginUser,
            IpAddress = ipAddress,
            ProvinceType = useProvince,
        };

        db.ProductUrlMonitorEntry.Add(productUrlMonitorEntry);
        db.SaveChanges();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
        });
    }
}