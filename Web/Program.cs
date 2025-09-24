using Core;
using Core.Config;
using Core.Web;
using Domain;
using NLog;
using NLog.Web;
using Service;

namespace Web;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

#if DEBUG
        builder.WebHost
            .UseUrls("http://*:5001")
            .UseKestrel();
#endif

        // ConfigurationManager configuration = builder.Configuration; 总是用 StaticConfigurationManager 得了
        // IWebHostEnvironment environment = builder.Environment; 总是用 #if DEBUG 来区分感觉更方便

#if DEBUG
        LogManager.Setup().LoadConfigurationFromFile("nlog.dev.config");
#else
        LogManager.Setup().LoadConfigurationFromFile("nlog.config");
#endif

        builder.Logging.AddNLogWeb();

        // 读取数据库配置
        DatabaseConfig.ReadConfig(StaticConfigurationManager.Configuration);

        var services = builder.Services;

        // Microsoft.Extensions.Caching.Memory.IMemoryCache
        services.AddMemoryCache();

        // System.Net.Http.HttpClient
        services.AddHttpClient();

        // 添加 DbContext 服务
        services.AddDbContext<Db>();

        // Core 中 服务类
        services.AddCoreServices();

        // Service 中 服务类
        services.AddLocalServices();

        services.Configure<CookiePolicyOptions>(options => {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => false;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddControllersWithViews();

        var app = builder.Build();

        app.UseExceptionHandler("/error");

        app.UseStaticFiles();

        app.UseRouting();

        app.UseMiddleware<NLogRequestPostedBodyMiddleware>(); // {aspnet-request-posted-body} 需要配置这个
        app.UseMiddleware<SessionIdCookieMiddleware>();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

#if DEBUG
        Console.WriteLine("started");
#endif

        app.Run();
    }
}