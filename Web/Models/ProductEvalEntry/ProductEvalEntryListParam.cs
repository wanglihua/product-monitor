using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductEvalEntry;

public class ProductEvalEntryListParam : ListParam
{
    [JsonProperty("productEvalId")]
    public long ProductEvalId { get; set; }
}

public class ProductEvalEntryListParamValidator : AbstractValidator<ProductEvalEntryListParam>
{
    public ProductEvalEntryListParamValidator()
    {
        RuleFor(x => x.ProductEvalId).Must(p => p > 0).WithMessage("试用目录Id不能为空!");
    }
}