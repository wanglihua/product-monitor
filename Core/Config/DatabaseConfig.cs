using Microsoft.Extensions.Configuration;

namespace Core.Config;

public static class DatabaseConfig
{
    /// <summary>
    /// 这个跟配置文件中的数据库连接字符串名称匹配
    ///     "ConnectionStrings": {
    ///         "default": "Data Source=localhost;Initial Catalog=product_monitor;Integrated Security=True;"
    ///     }
    /// </summary>
    private static string ConnectionStringName => "default";

    public static void ReadConfig(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString(ConnectionStringName) ?? string.Empty;

        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new Exception("数据库连接字符串配置不正确！");
        }
    }

    public static string ConnectionString { get; private set; } = "";
}