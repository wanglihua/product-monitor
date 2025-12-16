using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductSaleEntry;

public class ProductSaleEntryDetailParam
{
    [JsonProperty("productSaleEntryId")]
    public long ProductSaleEntryId { get; set; }

    [JsonProperty("productSaleId")]
    public long ProductSaleId { get; set; }
}

public class ProductSaleEntryDetailParamValidator : AbstractValidator<ProductSaleEntryDetailParam>
{
    public ProductSaleEntryDetailParamValidator()
    {
        RuleFor(e => e.ProductSaleId).Must((e, p) => {
            if (e.ProductSaleEntryId <= 0 && p <= 0)
            {
                return false;
            }

            return true;
        }).WithMessage("销售目录Id不能为空！");
    }
}