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
        RuleFor(e => e.ProductEvalId).Must((e, p) => {
            if (e.ProductEvalEntryId <= 0 && p <= 0)
            {
                return false;
            }

            return true;
        }).WithMessage("产品试用Id不能为空！");
    }
}