using Microsoft.Extensions.Configuration;

namespace Core.Config;

public static class StaticConfigurationManager
{
    public static IConfiguration Configuration { get; }

    static StaticConfigurationManager()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());

#if DEBUG
        configBuilder = configBuilder.AddJsonFile("appsettings.Development.json");
#else
        configBuilder = configBuilder.AddJsonFile("appsettings.json");
#endif

        Configuration = configBuilder.Build();
    }
}