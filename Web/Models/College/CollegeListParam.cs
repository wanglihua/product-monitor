using Core.Web;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Models.College;

public class CollegeListParam : ListParam
{
    [BindProperty(Name = "codeQuery")]
    [JsonProperty("codeQuery")]
    public string CodeQuery { get; set; } = "";

    [BindProperty(Name = "nameQuery")]
    [JsonProperty("nameQuery")]
    public string NameQuery { get; set; } = "";
}

public class CollegeListParamValidator : AbstractValidator<CollegeListParam>
{
    public CollegeListParamValidator()
    {
    }
}