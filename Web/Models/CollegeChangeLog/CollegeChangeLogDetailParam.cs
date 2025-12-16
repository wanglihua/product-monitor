using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.CollegeChangeLog;

public class CollegeChangeLogDetailParam
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }
}

public class CollegeChangeLogDetailParamValidator : AbstractValidator<CollegeChangeLogDetailParam>
{
    public CollegeChangeLogDetailParamValidator()
    {
    }
}