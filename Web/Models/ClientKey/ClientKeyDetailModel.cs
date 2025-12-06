using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ClientKey;

public class ClientKeyDetailModel
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("collegeId")]
    public long CollegeId { get; set; }

    [JsonProperty("collegeName")]
    public string CollegeName { get; set; } = "";

    [JsonProperty("productId")]
    public long ProductId { get; set; }

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("keyType")]
    public int KeyType { get; set; }

    [JsonProperty("clientKey")]
    public string ClientKey { get; set; } = "";
}

public class ClientKeyDetailModelValidator : AbstractValidator<ClientKeyDetailModel>
{
    public ClientKeyDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).NotNull().WithMessage("学校不能为空!");
        RuleFor(e => e.CollegeId).NotEmpty().WithMessage("学校不能为空!");

        RuleFor(e => e.ProductId).NotNull().WithMessage("产品不能为空!");
        RuleFor(e => e.ProductId).NotEmpty().WithMessage("产品不能为空!");

        RuleFor(e => e.KeyType)
            .Must(p => ClientKeyType.GetById(p) != ClientKeyType.None)
            .WithMessage("类型不能为空!");

        RuleFor(e => e.ClientKey).NotNull().WithMessage("密钥不能为空!");
        RuleFor(e => e.ClientKey).NotEmpty().WithMessage("密钥不能为空!");
        RuleFor(e => e.ClientKey).MaximumLength(128).WithMessage("密钥最大长度不能超过128!");
    }
}