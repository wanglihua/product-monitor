using Domain;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ClientKey;

public class ClientKeyDetailModel
{
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

    [JsonProperty("clientKeyTypeList")]
    public IList<Dictionary<string, object>> ClientKeyTypeList => ClientKeyType.GetList()
        .Select(e => new Dictionary<string, object> {
            ["value"] = e.Id,
            ["label"] = e.Name,
        })
        .ToList();
}

public class ClientKeyDetailModelValidator : AbstractValidator<ClientKeyDetailModel>
{
    public ClientKeyDetailModelValidator()
    {
        RuleFor(e => e.CollegeId).Must(p => p > 0).WithMessage("学校不能为空!");
        RuleFor(e => e.ProductId).Must(p => p > 0).WithMessage("产品不能为空!");

        RuleFor(e => e.KeyType)
            .Must(p => ClientKeyType.GetById(p) != ClientKeyType.None)
            .WithMessage("类型不能为空!");
    }
}