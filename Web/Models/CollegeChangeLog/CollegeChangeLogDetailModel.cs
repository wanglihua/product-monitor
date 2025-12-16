using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.CollegeChangeLog;

public class CollegeChangeLogDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("formerName")]
    public string FormerName { get; set; } = "";

    [JsonProperty("changeDesc")]
    public string ChangeDesc { get; set; } = "";
}

public class CollegeChangeLogDetailModelValidator : AbstractValidator<CollegeChangeLogDetailModel>
{
    public CollegeChangeLogDetailModelValidator()
    {
        RuleFor(e => e.CollegeId)
            .Must((e, p) => {
                if (e.Id <= 0)
                {
                    return p > 0;
                }

                return true;
            })
            .WithMessage("院校不能为空!");

        RuleFor(e => e.FormerName).NotNull().WithMessage("曾用名不能为空!");
        RuleFor(e => e.FormerName).NotEmpty().WithMessage("曾用名不能为空!");
        RuleFor(e => e.FormerName).MaximumLength(128).WithMessage("曾用名最大长度不能超过128!");

        RuleFor(e => e.ChangeDesc).MaximumLength(512).WithMessage("更新描述最大长度不能超过512!");
    }
}