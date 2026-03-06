using Core.Auth;
using Core.Config;
using Core.Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Web.Models.ProductSelect;

namespace Web.Controllers;

[Auth]
public class ProductSelectController(Db db) : Controller
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    [HttpPost]
    [Route("/api/product/select/data")]
    [Transactional(false)]
    public IActionResult Data([FromBody] ProductSelectDataQueryParam param)
    {
        var query = db.Product.AsNoTracking();

        if (string.IsNullOrWhiteSpace(param.Search) == false)
        {
            string search = param.Search.Trim();
            query = query.Where(e => e.Name.Contains(search));
        }

        var productList = query
            .OrderBy(e => e.Id)
            .Take(SelectConfig.DropdownItemMaxCount)
            .Select(e => new {
                Id = e.Id,
                Name = e.Name,
            })
            .ToList();

        var productItemList = productList
            .Select(e => new Dictionary<string, object>() {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }

    [HttpPost]
    [Route("/api/product/select/by/sale")]
    [Transactional(false)]
    public IActionResult BySale([FromBody] ProductSelectBySaleQueryParam param)
    {
        var query = db.ProductSale.AsNoTracking();

        long collegeId = param.CollegeId;
        query = query.Where(e => e.CollegeId == collegeId);

        var productIdList = query
            .OrderBy(e => e.Id)
            .Take(SelectConfig.DropdownItemMaxCount)
            .Select(e => e.ProductId)
            .ToList();

        var productList = db.Product.AsNoTracking()
            .Where(e => productIdList.Contains(e.Id))
            .OrderBy(e => e.Id)
            .ToList();

        var productItemList = productList
            .Select(e => new Dictionary<string, object>() {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }

    [HttpPost]
    [Route("/api/product/select/by/rent")]
    [Transactional(false)]
    public IActionResult ByRent([FromBody] ProductSelectByRentQueryParam param)
    {
        var query = db.ProductRent.AsNoTracking();

        long collegeId = param.CollegeId;
        query = query.Where(e => e.CollegeId == collegeId);

        var productIdList = query
            .OrderBy(e => e.Id)
            .Take(SelectConfig.DropdownItemMaxCount)
            .Select(e => e.ProductId)
            .ToList();

        var productList = db.Product.AsNoTracking()
            .Where(e => productIdList.Contains(e.Id))
            .OrderBy(e => e.Id)
            .ToList();

        var productItemList = productList
            .Select(e => new Dictionary<string, object>() {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }

    [HttpPost]
    [Route("/api/product/select/by/eval")]
    [Transactional(false)]
    public IActionResult ByEval([FromBody] ProductSelectByEvalQueryParam param)
    {
        var query = db.ProductEval.AsNoTracking();

        long collegeId = param.CollegeId;
        query = query.Where(e => e.CollegeId == collegeId);

        var productIdList = query
            .OrderBy(e => e.Id)
            .Take(SelectConfig.DropdownItemMaxCount)
            .Select(e => e.ProductId)
            .ToList();

        var productList = db.Product.AsNoTracking()
            .Where(e => productIdList.Contains(e.Id))
            .OrderBy(e => e.Id)
            .ToList();

        var productItemList = productList
            .Select(e => new Dictionary<string, object>() {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }

    [NonAction]
    private List<long> GetProductIdList(ProductSelectByProjectTypeQueryParam param)
    {
        var projectType = ProjectType.GetById(param.ProjectTypeId);

        if (projectType == ProjectType.Sale)
        {
            var query = db.ProductSale.AsNoTracking();

            long collegeId = param.CollegeId;
            query = query.Where(e => e.CollegeId == collegeId);

            var productIdList = query
                .OrderBy(e => e.Id)
                .Take(SelectConfig.DropdownItemMaxCount)
                .Select(e => e.ProductId)
                .ToList();

            return productIdList;
        }

        if (projectType == ProjectType.Rent)
        {
            var query = db.ProductRent.AsNoTracking();

            long collegeId = param.CollegeId;
            query = query.Where(e => e.CollegeId == collegeId);

            var productIdList = query
                .OrderBy(e => e.Id)
                .Take(SelectConfig.DropdownItemMaxCount)
                .Select(e => e.ProductId)
                .ToList();

            return productIdList;
        }

        if (projectType == ProjectType.Eval)
        {
            var query = db.ProductEval.AsNoTracking();

            long collegeId = param.CollegeId;
            query = query.Where(e => e.CollegeId == collegeId);

            var productIdList = query
                .OrderBy(e => e.Id)
                .Take(SelectConfig.DropdownItemMaxCount)
                .Select(e => e.ProductId)
                .ToList();

            return productIdList;
        }

        return new List<long>();
    }

    [HttpPost]
    [Route("/api/product/select/by/project/type")]
    [Transactional(false)]
    public IActionResult ByProjectType([FromBody] ProductSelectByProjectTypeQueryParam param)
    {
        var productIdList = GetProductIdList(param);

        var productList = db.Product.AsNoTracking()
            .Where(e => productIdList.Contains(e.Id))
            .OrderBy(e => e.Id)
            .ToList();

        var productItemList = productList
            .Select(e => new Dictionary<string, object>() {
                ["value"] = e.Id,
                ["label"] = e.Name,
            })
            .ToList();

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }
}