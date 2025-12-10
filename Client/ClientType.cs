namespace Client
{
    /// <summary>
    /// 客户端类型，是产品安装还是网站
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// 产品安装
        /// </summary>
        Install = 100,

        /// <summary>
        /// 试用网址
        /// </summary>
        UrlEval = 200,

        /// <summary>
        /// 租赁网址
        /// </summary>
        UrlRent = 300,

        /// <summary>
        /// 网站网址
        /// </summary>
        UrlSite = 400,
    }
}