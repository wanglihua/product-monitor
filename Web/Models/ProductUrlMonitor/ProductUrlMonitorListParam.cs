using Core.Web;
using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitor;

public class ProductUrlMonitorListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";

    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";

    [JsonProperty("productUrlTypeQuery")]
    [JsonConverter(typeof(ProductUrlTypeJsonConverter))]
    public ProductUrlType ProductUrlTypeQuery { get; set; } = ProductUrlType.None;
}

public class ProductUrlMonitorListParamValidator : AbstractValidator<ProductUrlMonitorListParam>
{
    public ProductUrlMonitorListParamValidator()
    {
    }
}