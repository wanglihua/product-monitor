using FluentValidation;
using Newtonsoft.Json;

namespace Core.Web;

public class IdParam
{
    [JsonProperty("id")]
    public long Id { get; set; }
}

public class IdParamValidator : AbstractValidator<IdParam>
{
    public IdParamValidator()
    {
        RuleFor(e => e.Id).GreaterThan(0).WithMessage("id不能为空！");
    }
}

public class IdParamEmptyValidator : AbstractValidator<IdParam>
{
    public IdParamEmptyValidator()
    {
    }
}