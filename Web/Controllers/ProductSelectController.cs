using Core.Auth;
using Core.Config;
using Core.Database;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Service;
using Web.Models.ProductSelect;

namespace Web.Controllers;

[Auth]
public class ProductSelectController(
    ProductSelectService productSelectService,
    Db db) : Controller
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
        var projectType = ProjectType.Sale;
        long collegeId = param.CollegeId;

        var productItemList = productSelectService.GetProductSelectOptionList(projectType, collegeId);

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
        var projectType = ProjectType.Rent;
        long collegeId = param.CollegeId;

        var productItemList = productSelectService.GetProductSelectOptionList(projectType, collegeId);

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
        var projectType = ProjectType.Eval;
        long collegeId = param.CollegeId;

        var productItemList = productSelectService.GetProductSelectOptionList(projectType, collegeId);

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }

    [HttpPost]
    [Route("/api/product/select/by/project/type")]
    [Transactional(false)]
    public IActionResult ByProjectType([FromBody] ProductSelectByProjectTypeQueryParam param)
    {
        var projectType = ProjectType.GetById(param.ProjectTypeId);
        long collegeId = param.CollegeId;

        var productItemList = productSelectService.GetProductSelectOptionList(projectType, collegeId);

        return Json(new Dictionary<string, object> {
            ["success"] = true,
            ["message"] = "success",
            ["productList"] = productItemList,
        });
    }
}