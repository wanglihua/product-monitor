using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductEvalEntry;

public class ProductEvalEntryDetailParam
{
    [JsonProperty("productEvalEntryId")]
    public long ProductEvalEntryId { get; set; }

    [JsonProperty("productEvalId")]
    public long ProductEvalId { get; set; }
}

public class ProductEvalEntryDetailParamValidator : AbstractValidator<ProductEvalEntryDetailParam>
{
    public ProductEvalEntryDetailParamValidator()
    {
        // 有可能只提供了 院校和产品 Id

        // RuleFor(e => e.ProductEvalId).Must((e, p) => {
        //     if (e.ProductEvalEntryId <= 0 && p <= 0)
        //     {
        //         return false;
        //     }
        //
        //     return true;
        // }).WithMessage("试用目录Id不能为空！");
    }
}