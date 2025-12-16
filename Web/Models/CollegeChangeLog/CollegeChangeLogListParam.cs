using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.CollegeChangeLog;

public class CollegeChangeLogListParam : ListParam
{
    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }
}

public class CollegeChangeLogListParamValidator : AbstractValidator<CollegeChangeLogListParam>
{
    public CollegeChangeLogListParamValidator()
    {
        RuleFor(e => e.CollegeId).Must(p => p > 0).WithMessage("院校Id不能为空！");
    }
}