using System.Linq.Expressions;
using Core;
using Core.Auth;
using Core.Database;
using Core.Util;
using Core.Web;
using Domain;
using Domain.PassRes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Service;
using Web.Models.ClientKey;

namespace Web.Controllers;

[Auth(PassRes.SystemManage.ClientKey)]
public class ClientKeyController(
    Db db,
    UserLoginService userLoginService,
    ActionLogService actionLogService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ClientKeyJoinItem
    {
        public ClientKeyEntity ClientKey { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [HttpPost]
    [Route("/api/client-key/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ClientKeyListParam param)
    {
        ValidationUtil.Validate<ClientKeyListParamValidator, ClientKeyListParam>(param);

        IQueryable<ClientKeyJoinItem> query = db.ClientKey
            .Join(db.College,
                ck => ck.CollegeId,
                c => c.Id,
                (ck, c) => new ClientKeyJoinItem {
                    ClientKey = ck,
                    College = c,
                })
            .Join(db.Product,
                ck => ck.ClientKey.ProductId,
                p => p.Id,
                (ck, p) => new ClientKeyJoinItem {
                    ClientKey = ck.ClientKey,
                    College = ck.College,
                    Product = p,
                })
            .Join(db.User,
                ck => ck.ClientKey.UserId,
                u => u.Id,
                (ck, u) => new ClientKeyJoinItem {
                    ClientKey = ck.ClientKey,
                    College = ck.College,
                    Product = ck.Product,
                    User = u,
                })
            .AsNoTracking();

        int clientKeyCount = query.Count();

        // 排序字段
        Expression<Func<ClientKeyJoinItem, object>> sortField = e => e.ClientKey.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ClientKeyListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ClientKey.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ClientKeyListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ClientKey.UpdateTime;
            }
        }

        // 排序顺序
        string sortOrder = param.SortOrder;
        if (sortOrder == SortOrder.Ascend)
        {
            query = query.OrderBy(sortField);
        }
        else
        {
            query = query.OrderByDescending(sortField);
        }

        var clientKeyList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.ClientKey.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                KeyType = e.ClientKey.KeyType,
                ClientKey = e.ClientKey,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                CreateTime = e.ClientKey.CreateTime,
                UpdateTime = e.ClientKey.UpdateTime,
            })
            .ToList();

        var clientKeyItemList = clientKeyList
            .Select(e => new ClientKeyListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                KeyType = e.KeyType.Name,
                ClientKey = e.ClientKey.ClientKey,
                UserName = $"{e.LoginName}-{e.UserName}",
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["clientKeyList"] = clientKeyItemList,
                ["clientKeyCount"] = clientKeyCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/client-key/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long clientKeyId = param.Id;

        var model = new ClientKeyDetailModel {
            Id = 0,
            CollegeId = 0,
            CollegeName = "",
            ProductId = 0,
            ProductName = "",
            KeyType = 0,
            ClientKey = "",
        };

        if (clientKeyId > 0)
        {
            var clientKey = db.Set<ClientKeyEntity>().Find(clientKeyId);
            if (clientKey is null)
            {
                
            }

            if (clientKey is not null)
            {
                model = new ClientKeyDetailModel {
                    Id = clientKey.Id,
                    CollegeId = clientKey.CollegeId,
                    ProductId = clientKey.ProductId,
                    KeyType = clientKey.KeyType.Id,
                    ClientKey = clientKey.ClientKey,
                };
            }
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    [HttpPost]
    [Route("/api/client-key/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ClientKeyDetailModel model)
    {
        ValidationUtil.Validate<ClientKeyDetailModelValidator, ClientKeyDetailModel>(model);

        long clientKeyId = model.Id;
        bool isCreate = clientKeyId == 0;

        ClientKeyEntity? clientKey;
        if (isCreate == true)
        {
            clientKey = new ClientKeyEntity();
        }
        else
        {
            clientKey = db.Set<ClientKeyEntity>().Find(clientKeyId);
            if (clientKey is null)
            {
                throw new ValidationException($"指定的客户端密钥#{clientKeyId}不存在!");
            }
        }

        clientKey.CollegeId = model.CollegeId;
        clientKey.ProductId = model.ProductId;
        clientKey.KeyType = model.KeyType;
        clientKey.ClientKey = model.ClientKey ?? "";
        clientKey.UserId = model.UserId;

        if (isCreate == true)
        {
            db.Add<ClientKeyEntity>(clientKey);
        }

        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        if (isCreate == true)
        {
            string actionDesc = $"{clientKey.ClientKey}#{clientKey.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ClientKeyCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"{clientKey.ClientKey}#{clientKey.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ClientKeyUpdate, actionDesc);
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/client-key/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long clientKeyId = param.Id;
        var clientKey = db.Set<ClientKeyEntity>().Find(clientKeyId);
        if (clientKey is null)
        {
            throw new ValidationException($"指定的客户端密钥#{clientKeyId}不存在!");
        }

        db.Remove<ClientKeyEntity>(clientKey);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"{clientKey.ClientKey}#{clientKey.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ClientKeyDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }
}