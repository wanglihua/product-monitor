using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductInstallMonitorEntry;

public class ProductInstallMonitorEntryListParam : ListParam
{
    [JsonProperty("productInstallMonitorId")]
    public long ProductInstallMonitorId { get; set; }
}

public class ProductInstallMonitorEntryListParamValidator : AbstractValidator<ProductInstallMonitorEntryListParam>
{
    public ProductInstallMonitorEntryListParamValidator()
    {
        RuleFor(e => e.ProductInstallMonitorId).Must(p => p > 0).WithMessage("安装检测Id不能为空!");
    }
}