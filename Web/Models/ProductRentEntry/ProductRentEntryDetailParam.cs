using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductRentEntry;

public class ProductRentEntryDetailParam
{
    [JsonProperty("productRentEntryId")]
    public long ProductRentEntryId { get; set; }

    [JsonProperty("productRentId")]
    public long ProductRentId { get; set; }
}

public class ProductRentEntryDetailParamValidator : AbstractValidator<ProductRentEntryDetailParam>
{
    public ProductRentEntryDetailParamValidator()
    {
        // 有可能只提供了 院校和产品 Id
        
        // RuleFor(e => e.ProductRentId).Must((e, p) => {
        //     if (e.ProductRentEntryId <= 0 && p <= 0)
        //     {
        //         return false;
        //     }
        //
        //     return true;
        // }).WithMessage("租赁目录Id不能为空！");
    }
}