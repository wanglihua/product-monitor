using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductRentEntry;

public class ProductRentEntryListParam : ListParam
{
    [JsonProperty("productRentId")]
    public long ProductRentId { get; set; }
}

public class ProductRentEntryListParamValidator : AbstractValidator<ProductRentEntryListParam>
{
    public ProductRentEntryListParamValidator()
    {
        RuleFor(x => x.ProductRentId).Must(p => p > 0).WithMessage("租赁目录Id不能为空!");
    }
}