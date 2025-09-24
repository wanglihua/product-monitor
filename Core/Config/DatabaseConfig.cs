using Microsoft.Extensions.Configuration;

namespace Core.Config;

public static class DatabaseConfig
{
    public static void ReadConfig(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("default") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new Exception("数据库连接字符串配置不正确！");
        }
    }

    public static string ConnectionString { get; private set; } = string.Empty;
}