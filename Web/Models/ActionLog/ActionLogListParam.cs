using Core.Web;
using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ActionLog;

public class ActionLogListParam : ListParam
{
    [JsonProperty("loginNameQuery")]
    public string LoginNameQuery { get; set; } = "";

    [JsonProperty("actionLogTypeQuery")]
    [JsonConverter(typeof(ActionLogTypeJsonConverter))]
    public ActionLogType ActionLogTypeQuery { get; set; } = ActionLogType.None;
}

public class ActionLogListParamValidator : AbstractValidator<ActionLogListParam>
{
    public ActionLogListParamValidator()
    {
    }
}