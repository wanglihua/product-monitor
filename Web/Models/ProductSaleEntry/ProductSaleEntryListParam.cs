using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductSaleEntry;

public class ProductSaleEntryListParam : ListParam
{
    [JsonProperty("productSaleId")]
    public long ProductSaleId { get; set; }

    [JsonProperty("versionQuery")]
    public string VersionQuery { get; set; } = "";
}

public class ProductSaleEntryListParamValidator : AbstractValidator<ProductSaleEntryListParam>
{
    public ProductSaleEntryListParamValidator()
    {
        RuleFor(x => x.ProductSaleId).Must(p => p > 0).WithMessage("销售目录Id不能为空!");
    }
}