using Core.Web;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Models.ClientKey;

public class ClientKeyListParam : ListParam
{
    [BindProperty(Name = "collegeNameQuery")]
    [JsonProperty("collegeNameQuery")]
    public string CollegeNameQuery { get; set; } = "";

    [BindProperty(Name = "productNameQuery")]
    [JsonProperty("productNameQuery")]
    public string ProductNameQuery { get; set; } = "";
}

public class ClientKeyListParamValidator : AbstractValidator<ClientKeyListParam>
{
    public ClientKeyListParamValidator()
    {
    }
}