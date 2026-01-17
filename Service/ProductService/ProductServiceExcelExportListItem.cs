namespace Service.ProductService;

public class ProductServiceExcelExportListItem
{
    public long Id { get; set; }

    public string CollegeName { get; set; } = "";

    public string ProductName { get; set; } = "";

    public string ProjectTypeName { get; set; } = "";

    public string ProblemTypeName { get; set; } = "";

    public string ServiceDate { get; set; } = "";

    public int ServiceCount { get; set; }

    public string ServiceUrl { get; set; } = "";

    public string LoginUser { get; set; } = "";

    public string LoginPassword { get; set; } = "";

    public string ContactTeacher { get; set; } = "";

    public string ContactMobile { get; set; } = "";

    public string Description { get; set; } = "";

    public string CreateTime { get; set; } = "";

    public string UpdateTime { get; set; } = "";

    public string UserName { get; set; } = "";
}