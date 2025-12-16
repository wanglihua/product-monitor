using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.CollegeChangeLog;

public class CollegeChangeLogListParam : ListParam
{
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}

public class CollegeChangeLogListParamValidator : AbstractValidator<CollegeChangeLogListParam>
{
    public CollegeChangeLogListParamValidator()
    {
    }
}