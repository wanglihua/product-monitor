
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
    public string Province { get; set; } = ""; 

    [JsonProperty("level")]
    public string Level { get; set; } = ""; 

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

        RuleFor(e => e.Province).NotNull().WithMessage("身份不能为空!");
        RuleFor(e => e.Province).NotEmpty().WithMessage("身份不能为空!");
        RuleFor(e => e.Province).MaximumLength(32).WithMessage("身份最大长度不能超过32!");

        RuleFor(e => e.Level).NotNull().WithMessage("办学层次不能为空!");
        RuleFor(e => e.Level).NotEmpty().WithMessage("办学层次不能为空!");
        RuleFor(e => e.Level).MaximumLength(32).WithMessage("办学层次最大长度不能超过32!");

    }
}
