
using Core.Web;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Models.ProductSale;

public class ProductSaleListParam : ListParam
{
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";
    
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ProductSaleListParamValidator : AbstractValidator<ProductSaleListParam>
{
    public ProductSaleListParamValidator()
    {
    }
}