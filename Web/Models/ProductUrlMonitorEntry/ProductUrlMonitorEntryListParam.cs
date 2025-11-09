using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductUrlMonitorEntry;

public class ProductUrlMonitorEntryListParam : ListParam
{
    [JsonProperty("productUrlMonitorId")]
    public long ProductUrlMonitorId { get; set; }
}

public class ProductUrlMonitorEntryListParamValidator : AbstractValidator<ProductUrlMonitorEntryListParam>
{
    public ProductUrlMonitorEntryListParamValidator()
    {
        RuleFor(e => e.ProductUrlMonitorId).Must(p => p > 0).WithMessage("网站检测Id不能为空!");
    }
}