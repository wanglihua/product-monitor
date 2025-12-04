using Microsoft.Extensions.Configuration;

namespace Core.Config;

public static class WebServerConfig
{
    /// <summary>
    /// 配置值 IIS
    /// </summary>
    public static string IIS => "IIS";

    /// <summary>
    /// 配置值 Kestrel
    /// </summary>
    public static string Kestrel => "Kestrel";

    private static string WebServerConfigName => "WebServer";

    public static void ReadConfig(IConfiguration configuration)
    {
        WebServer = configuration[WebServerConfigName] ?? string.Empty;

        if (string.IsNullOrWhiteSpace(WebServer))
        {
            throw new Exception("WebServer配置不正确！");
        }
    }

    /// <summary>
    /// IIS 或者 Kestrel
    /// </summary>
    public static string WebServer { get; private set; } = "";
}