using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitor;

public class ProductInstallMonitorListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductInstallMonitorListParamValidator : AbstractValidator<ProductInstallMonitorListParam>
{
    public ProductInstallMonitorListParamValidator()
    {
    }
}