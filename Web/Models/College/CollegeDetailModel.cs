using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.College;

public class CollegeDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; } = "";

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("province")]
    public int Province { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("provinceList")]
    public IList<Dictionary<string, object>> ProvinceList => ProvinceType.GetList()
        .Select(e => new Dictionary<string, object> {
            ["value"] = e.Id,
            ["label"] = e.Name,
        }).ToList();

    [JsonProperty("levelList")]
    public IList<Dictionary<string, object>> LevelList => CollegeLevelType.GetList()
        .Select(e => new Dictionary<string, object> {
            ["value"] = e.Id,
            ["label"] = e.Name,
        })
        .ToList();
}

public class CollegeDetailModelValidator : AbstractValidator<CollegeDetailModel>
{
    public CollegeDetailModelValidator()
    {
        RuleFor(e => e.Code).NotNull().WithMessage("院校标识码不能为空!");
        RuleFor(e => e.Code).NotEmpty().WithMessage("院校标识码不能为空!");
        RuleFor(e => e.Code).MaximumLength(64).WithMessage("院校标识码最大长度不能超过64!");

        RuleFor(e => e.Name).NotNull().WithMessage("院校名称不能为空!");
        RuleFor(e => e.Name).NotEmpty().WithMessage("院校名称不能为空!");
        RuleFor(e => e.Name).MaximumLength(128).WithMessage("院校名称最大长度不能超过128!");

        RuleFor(e => e.Province).Must(p => ProvinceType.GetById(p) != ProvinceType.None).WithMessage("身份不能为空!");
        RuleFor(e => e.Level).Must(p => CollegeLevelType.GetById(p) != CollegeLevelType.None).WithMessage("办学层次不能为空!");
    }
}