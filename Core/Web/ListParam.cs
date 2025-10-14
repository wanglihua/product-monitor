using Newtonsoft.Json;

namespace Core.Web;

public class ListParam
{
    /// <summary>
    /// 当前分页，第一页为 1
    /// </summary>
    [JsonProperty("currentPage")]
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// 当前分页，第一页为 0
    /// </summary>
    [JsonIgnore]
    public int PageIndex => CurrentPage - 1;

    /// <summary>
    /// 分页大小
    /// </summary>
    [JsonProperty("pageSize")]
    public int PageSize { get; set; } = 15;

    /// <summary>
    /// 排序字段
    /// </summary>
    [JsonProperty("sortField")]
    public string SortField { get; set; } = "";

    /// <summary>
    /// 排序顺序
    /// </summary>
    [JsonProperty("sortOrder")]
    public string SortOrder { get; set; } = "";
}