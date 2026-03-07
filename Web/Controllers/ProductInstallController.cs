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
using Service.ProductEval;
using Service.ProductInstall;
using Service.ProductRent;
using Service.ProductSale;
using Web.Models.ProductInstall;

namespace Web.Controllers;

[Auth(PassRes.ProductService.ProductInstall)]
public class ProductInstallController(
    IWebHostEnvironment webHostEnvironment,
    Db db,
    UserLoginService userLoginService,
    UserProvinceService userProvinceService,
    ProductSaleService productSaleService,
    ProductRentService productRentService,
    ProductEvalService productEvalService,
    ActionLogService actionLogService,
    ProductInstallExcelService productInstallExcelService) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    private class ProductInstallJoinItem
    {
        public ProductInstallEntity ProductInstall { get; set; } = new();

        public CollegeEntity College { get; set; } = new();

        public ProductEntity Product { get; set; } = new();

        public UserEntity User { get; set; } = new();
    }

    [NonAction]
    private IQueryable<ProductInstallJoinItem> GetListQuery(ProductInstallListParam param)
    {
        ValidationUtil.Validate<ProductInstallListParamValidator, ProductInstallListParam>(param);

        long loginUserId = userLoginService.GetLoginUserId();
        bool isAdminLogin = userLoginService.IsAdmin(loginUserId);

        IQueryable<ProductInstallJoinItem> query = db.ProductInstall
            .Join(db.College,
                pi => pi.CollegeId,
                c => c.Id,
                (pi, c) => new ProductInstallJoinItem {
                    ProductInstall = pi,
                    College = c,
                })
            .Join(db.Product,
                pi => pi.ProductInstall.ProductId,
                p => p.Id,
                (pi, p) => new ProductInstallJoinItem {
                    ProductInstall = pi.ProductInstall,
                    College = pi.College,
                    Product = p,
                })
            .Join(db.User,
                pi => pi.ProductInstall.UserId,
                u => u.Id,
                (pi, u) => new ProductInstallJoinItem {
                    ProductInstall = pi.ProductInstall,
                    College = pi.College,
                    Product = pi.Product,
                    User = u,
                })
            .AsNoTracking();

        if (isAdminLogin == false)
        {
            var userProvinceList = userProvinceService.GetUserProvinceList(loginUserId);
            query = query.Where(e => userProvinceList.Contains(e.College.Province));
        }

        if (string.IsNullOrWhiteSpace(param.CollegeNameQuery) == false)
        {
            string collegeNameQuery = param.CollegeNameQuery.Trim();
            query = query.Where(e => e.College.Name.Contains(collegeNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.ProductNameQuery) == false)
        {
            string productNameQuery = param.ProductNameQuery.Trim();
            query = query.Where(e => e.Product.Name.Contains(productNameQuery));
        }

        if (string.IsNullOrWhiteSpace(param.InstallDateBeginQuery) == false)
        {
            string installDateBeginQuery = param.InstallDateBeginQuery.Trim();
            if (DateTime.TryParse(installDateBeginQuery, out DateTime installDateBegin) == true)
            {
                query = query.Where(e => e.ProductInstall.InstallDate >= installDateBegin);
            }
        }

        if (string.IsNullOrWhiteSpace(param.InstallDateEndQuery) == false)
        {
            string installDateEndQuery = param.InstallDateEndQuery.Trim();
            if (DateTime.TryParse(installDateEndQuery, out DateTime installDateEnd) == true)
            {
                installDateEnd = installDateEnd.AddDays(1);
                query = query.Where(e => e.ProductInstall.InstallDate < installDateEnd);
            }
        }

        return query;
    }

    [HttpPost]
    [Route("/api/product-install/list")]
    [Transactional(false)]
    public IActionResult List([FromBody] ProductInstallListParam param)
    {
        IQueryable<ProductInstallJoinItem> query = GetListQuery(param);

        int productInstallCount = query.Count();

        // 排序字段
        Expression<Func<ProductInstallJoinItem, object>> sortField = e => e.ProductInstall.Id;
        if (string.IsNullOrWhiteSpace(param.SortField) == false)
        {
            string sortFieldParam = param.SortField;

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.CollegeName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.College.Name;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.ProductName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.Product.Name;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.VersionSale), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstall.VersionSale;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.VersionInstall), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstall.VersionInstall;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.InstallDate), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstall.InstallDate;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.UserName), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.User.Id;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.CreateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstall.CreateTime;
            }

            if (string.Compare(sortFieldParam, nameof(ProductInstallListItem.UpdateTime), StringComparison.OrdinalIgnoreCase) == 0)
            {
                sortField = e => e.ProductInstall.UpdateTime;
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

        var productInstallList = query
            .Skip(param.PageIndex * param.PageSize)
            .Take(param.PageSize)
            .Select(e => new {
                Id = e.ProductInstall.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                VersionSale = e.ProductInstall.VersionSale,
                VersionInstall = e.ProductInstall.VersionInstall,
                InstallDate = e.ProductInstall.InstallDate,
                UpdateUrl = e.ProductInstall.UpdateUrl,
                LoginUser = e.ProductInstall.LoginUser,
                LoginPassword = e.ProductInstall.LoginPassword,
                ContactTeacher = e.ProductInstall.ContactTeacher,
                ContactMobile = e.ProductInstall.ContactMobile,
                Description = e.ProductInstall.Description,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                CreateTime = e.ProductInstall.CreateTime,
                UpdateTime = e.ProductInstall.UpdateTime,
            })
            .ToList();

        var productInstallItemList = productInstallList
            .Select(e => new ProductInstallListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                VersionSale = e.VersionSale,
                VersionInstall = e.VersionInstall,
                InstallDate = e.InstallDate.ToString("yyyy-MM-dd"),
                UpdateUrl = e.UpdateUrl,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                ContactTeacher = e.ContactTeacher,
                ContactMobile = e.ContactMobile,
                Description = e.Description,
                UserName = $"{e.LoginName}-{e.UserName}",
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = new Dictionary<string, object> {
                ["productInstallList"] = productInstallItemList,
                ["productInstallCount"] = productInstallCount,
            },
        });
    }

    [HttpPost]
    [Route("/api/product-install/detail")]
    [Transactional(false)]
    public IActionResult Detail([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamEmptyValidator, IdParam>(param);

        long productInstallId = param.Id;

        var model = new ProductInstallDetailModel {
            Id = 0,
            CollegeId = 0,
            CollegeName = "",
            ProductId = 0,
            ProductName = "",
            VersionSale = null,
            VersionInstall = null,
            InstallDate = DateTime.Today.ToString("yyyy-MM-dd"),
            UpdateUrl = "",
            LoginUser = "",
            LoginPassword = "",
            ContactTeacher = "",
            ContactMobile = "",
            Description = "",
        };

        if (productInstallId > 0)
        {
            var productInstall = db.ProductInstall.Find(productInstallId);
            if (productInstall is null)
            {
                throw new ValidationException($"指定的产品安装#{productInstallId}不存在!");
            }

            long collegeId = productInstall.CollegeId;
            var college = db.College.Find(collegeId);
            if (college is null)
            {
                throw new ValidationException($"指定的学校#{collegeId}不存在!");
            }

            long productId = productInstall.ProductId;
            var product = db.Product.Find(productId);
            if (product is null)
            {
                throw new ValidationException($"指定的产品#{productId}不存在!");
            }

            // 处理版本下拉框
            var productVersionList = db.ProductVersion.AsNoTracking()
                .Where(e => e.ProductId == productId)
                .OrderBy(e => e.Version)
                .ToList();

            var productVersionItemList = productVersionList
                .Select(e => new Dictionary<string, object> {
                    ["value"] = e.Version,
                    ["label"] = e.Version,
                })
                .ToList();

            // 已经录入的数据如果列表中没有放到下拉框中
            string versionSale = productInstall.VersionSale;
            if (string.IsNullOrWhiteSpace(versionSale) == false)
            {
                if (productVersionList.Any(e => e.Version == versionSale) == false)
                {
                    productVersionItemList.Add(new Dictionary<string, object> {
                        ["value"] = versionSale,
                        ["label"] = versionSale,
                    });
                }
            }

            string versionInstall = productInstall.VersionInstall;
            if (string.IsNullOrWhiteSpace(versionInstall) == false && versionInstall != versionSale)
            {
                if (productVersionList.Any(e => e.Version == versionInstall) == false)
                {
                    productVersionItemList.Add(new Dictionary<string, object> {
                        ["value"] = versionInstall,
                        ["label"] = versionInstall,
                    });
                }
            }

            // var productIdList = db.pro.AsNoTracking()
            //     .Select(e => e.ProductId)
            //     .ToList();

            // 附件
            var attachment = db.ProductInstallAttachment.AsNoTracking()
                .Where(e => e.ProductInstallId == productInstallId)
                .OrderByDescending(e => e.Id)
                .Take(1)
                .SingleOrDefault();

            model = new ProductInstallDetailModel {
                Id = productInstall.Id,
                CollegeId = collegeId,
                CollegeName = college.Name,
                ProductId = productId,
                ProductName = product.Name,
                VersionSale = versionSale,
                VersionInstall = versionInstall,
                InstallDate = productInstall.InstallDate.ToString("yyyy-MM-dd"),
                UpdateUrl = productInstall.UpdateUrl,
                LoginUser = productInstall.LoginUser,
                LoginPassword = productInstall.LoginPassword,
                ContactTeacher = productInstall.ContactTeacher,
                ContactMobile = productInstall.ContactMobile,
                Description = productInstall.Description,
                ProductVersionList = productVersionItemList,
                AttachmentId = attachment?.Id ?? 0,
                AttachmentName = attachment?.Name ?? "",
            };
        }

        return Json(new JsonResultData {
            Success = true,
            Message = "success",
            Data = model,
        });
    }

    private static readonly string uploadFolder = Path.Combine("upload", "product-install");

    [HttpPost]
    [Route("/api/product-install/attachment/upload/{productInstallId:long}")]
    [Transactional(false)]
    public IActionResult AttachmentUpload(long productInstallId, IFormFile attachmentFile)
    {
        if (attachmentFile is null)
        {
            throw new ValidationException("文件上传失败，请重新上传");
        }

        var productInstall = db.ProductInstall.Find(productInstallId);
        if (productInstall is null)
        {
            throw new ValidationException($"指定的产品安装#{productInstallId}不存在");
        }

        string fileName = attachmentFile.FileName;
        string fileExt = new FileInfo(fileName).Extension;

        string newFileName = Guid.NewGuid().ToString("N") + fileExt;
        string monthFolderName = DateTime.Now.ToString("yyyy-MM");
        string fullUploadFolder = Path.Combine(webHostEnvironment.ContentRootPath, uploadFolder, monthFolderName);
        string newFileFullName = Path.Combine(fullUploadFolder, newFileName);

        if (Directory.Exists(fullUploadFolder) == false)
        {
            Directory.CreateDirectory(fullUploadFolder);
        }

        using Stream newFileStream = System.IO.File.Create(newFileFullName);
        attachmentFile.CopyTo(newFileStream);

        long attachmentId = 0L;
        Transactional.Wrap(db, () => {
            var attachment = new ProductInstallAttachmentEntity {
                ProductInstallId = productInstallId,
                Name = fileName,
                PathFile = Path.Combine(monthFolderName, newFileName),
            };

            db.ProductInstallAttachment.Add(attachment);

            db.SaveChanges();

            attachmentId = attachment.Id;
        });

        return Json(new JsonResultData {
            Success = true,
            Message = "文件上传并已保存成功",
            Data = new Dictionary<string, object> {
                ["attachmentId"] = attachmentId,
                ["attachmentName"] = fileName,
            }
        });
    }

    [HttpPost]
    [Route("/api/product-install/attachment/remove")]
    [Transactional(true)]
    public IActionResult AttachmentUpload([FromBody] ProductInstallAttachmentRemoveParam param)
    {
        long attachmentId = param.AttachmentId;
        var attachment = db.ProductInstallAttachment.Find(attachmentId);
        if (attachment is null)
        {
            throw new ValidationException($"指定的安装附件#{attachmentId}不存在");
        }

        string fullFileName = Path.Combine(webHostEnvironment.ContentRootPath, uploadFolder, attachment.PathFile);

        db.ProductInstallAttachment.Remove(attachment);
        db.SaveChanges();

        System.IO.File.Delete(fullFileName);

        return Json(new JsonResultData {
            Success = true,
            Message = "附件移除成功",
        });
    }

    [HttpGet]
    [Route("/api/product-install/attachment/download/{attachmentId:long}")]
    [Transactional(false)]
    public IActionResult AttachmentDownload(long attachmentId)
    {
        var attachment = db.ProductInstallAttachment.Find(attachmentId);
        if (attachment is null)
        {
            throw new Exception($"指定的产品安装附件#{attachmentId}不存在！");
        }

        string fileName = attachment.Name;
        string fullFileName = Path.Combine(webHostEnvironment.ContentRootPath, uploadFolder, attachment.PathFile);

        // 用 zip 是让总是提示下载，而不是碰到图片类型时直接打开一个图片
        return File(System.IO.File.OpenRead(fullFileName), ContentType.Zip, fileName);
    }

    [HttpPost]
    [Route("/api/product-install/save")]
    [Transactional(true)]
    public IActionResult Save([FromBody] ProductInstallDetailModel model)
    {
        ValidationUtil.Validate<ProductInstallDetailModelValidator, ProductInstallDetailModel>(model);

        long loginUserId = userLoginService.GetLoginUserId();

        long productInstallId = model.Id;
        bool isCreate = productInstallId == 0;

        ProductInstallEntity? productInstall;
        if (isCreate == true)
        {
            productInstall = new ProductInstallEntity();
        }
        else
        {
            productInstall = db.ProductInstall.Find(productInstallId);
            if (productInstall is null)
            {
                throw new ValidationException($"指定的产品安装#{productInstallId}不存在!");
            }
        }

        long collegeId = model.CollegeId;
        long productId = model.ProductId;

        // 检查 学校 和 产品 关联
        var productSale = db.ProductSale.AsNoTracking()
            .Where(e => e.CollegeId == collegeId && e.ProductId == productId)
            .Take(1)
            .SingleOrDefault();
        if (productSale is null)
        {
            throw new ValidationException("指定的学校和产品组合在销售目录中不存在！");
        }

        productInstall.CollegeId = collegeId;
        productInstall.ProductId = productId;
        productInstall.VersionSale = model.VersionSale ?? "";
        productInstall.VersionInstall = model.VersionInstall ?? "";
        productInstall.InstallDate = DateTime.Parse(model.InstallDate).Date;
        productInstall.UpdateUrl = model.UpdateUrl ?? "";
        productInstall.LoginUser = model.LoginUser ?? "";
        productInstall.LoginPassword = model.LoginPassword ?? "";
        productInstall.ContactTeacher = model.ContactTeacher ?? "";
        productInstall.ContactMobile = model.ContactMobile ?? "";
        productInstall.Description = model.Description ?? "";
        productInstall.UserId = loginUserId;

        if (isCreate == true)
        {
            db.ProductInstall.Add(productInstall);
        }

        db.SaveChanges();

        if (isCreate == true)
        {
            string actionDesc = $"#{productInstall.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductInstallCreate, actionDesc);
        }
        else
        {
            string actionDesc = $"#{productInstall.Id}";
            actionLogService.Add(loginUserId, ActionLogType.ProductInstallUpdate, actionDesc);
        }

        productInstallId = productInstall.Id;

        productSaleService.UpdateProductSaleStatisticsDataByProductInstall(productInstallId);
        productRentService.UpdateProductRentStatisticsDataByProductInstall(productInstallId);
        productEvalService.UpdateProductEvalStatisticsDataByProductInstall(productInstallId);

        return Json(new JsonResultData {
            Success = true,
            Message = "保存成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-install/delete")]
    [Transactional(true)]
    public IActionResult Delete([FromBody] IdParam param)
    {
        ValidationUtil.Validate<IdParamValidator, IdParam>(param);

        long productInstallId = param.Id;
        var productInstall = db.ProductInstall.Find(productInstallId);
        if (productInstall is null)
        {
            throw new ValidationException($"指定的产品安装#{productInstallId}不存在!");
        }

        db.ProductInstall.Remove(productInstall);
        db.SaveChanges();

        long loginUserId = userLoginService.GetLoginUserId();
        string actionDesc = $"#{productInstall.Id}";
        actionLogService.Add(loginUserId, ActionLogType.ProductInstallDelete, actionDesc);

        return Json(new JsonResultData {
            Success = true,
            Message = "删除成功！",
        });
    }

    [HttpPost]
    [Route("/api/product-install/export")]
    [Transactional(false)]
    public IActionResult Export(ProductInstallListParam param)
    {
        ValidationUtil.Validate<ProductInstallListParamValidator, ProductInstallListParam>(param);

        IQueryable<ProductInstallJoinItem> query = GetListQuery(param);

        var productInstallList = query
            .OrderBy(e => e.ProductInstall.Id)
            .Select(e => new {
                Id = e.ProductInstall.Id,
                CollegeName = e.College.Name,
                ProductName = e.Product.Name,
                VersionSale = e.ProductInstall.VersionSale,
                VersionInstall = e.ProductInstall.VersionInstall,
                InstallDate = e.ProductInstall.InstallDate,
                UpdateUrl = e.ProductInstall.UpdateUrl,
                LoginUser = e.ProductInstall.LoginUser,
                LoginPassword = e.ProductInstall.LoginPassword,
                ContactTeacher = e.ProductInstall.ContactTeacher,
                ContactMobile = e.ProductInstall.ContactMobile,
                Description = e.ProductInstall.Description,
                LoginName = e.User.LoginName,
                UserName = e.User.UserName,
                CreateTime = e.ProductInstall.CreateTime,
                UpdateTime = e.ProductInstall.UpdateTime,
            })
            .ToList();

        var productInstallItemList = productInstallList
            .Select(e => new ProductInstallExcelExportListItem {
                Id = e.Id,
                CollegeName = e.CollegeName,
                ProductName = e.ProductName,
                VersionSale = e.VersionSale,
                VersionInstall = e.VersionInstall,
                InstallDate = e.InstallDate.ToString("yyyy-MM-dd"),
                UpdateUrl = e.UpdateUrl,
                LoginUser = e.LoginUser,
                LoginPassword = e.LoginPassword,
                ContactTeacher = e.ContactTeacher,
                ContactMobile = e.ContactMobile,
                Description = e.Description,
                UserName = $"{e.LoginName}-{e.UserName}",
                CreateTime = e.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd HH:mm"),
            })
            .ToList();

        var excelContent = productInstallExcelService.Export(productInstallItemList);

        return File(excelContent, ContentType.Xlsx, "产品安装.xlsx");
    }
}