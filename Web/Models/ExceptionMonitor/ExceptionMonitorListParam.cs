using Core.CodeType;
using Core.Web;
using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ExceptionMonitor;

public class ExceptionMonitorListParam : ListParam
{
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";

    [JsonProperty("provinceTypeQuery")]
    [JsonConverter(typeof(ProvinceTypeJsonConverter))]
    public ProvinceType ProvinceTypeQuery { get; set; } = ProvinceType.None;
}

public class ExceptionMonitorListParamValidator : AbstractValidator<ExceptionMonitorListParam>
{
    public ExceptionMonitorListParamValidator()
    {
    }
}