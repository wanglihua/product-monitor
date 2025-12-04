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

    private static string SqlServerVersionConfigName => "SQLServerVersion";

    public static void ReadConfig(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString(ConnectionStringName) ?? string.Empty;

        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new Exception("数据库连接字符串配置不正确！");
        }

        SqlVersion = configuration[SqlServerVersionConfigName] ?? string.Empty;

        if (string.IsNullOrWhiteSpace(SqlVersion))
        {
            throw new Exception("SQLServerVersion配置不正确！");
        }
    }

    public static string ConnectionString { get; private set; } = "";

    /// <summary>
    /// 可能的配置值是 2008，2012, 2014, 2016, 2017, 2019, 2022 等
    /// </summary>
    public static string SqlVersion { get; private set; } = "";
}