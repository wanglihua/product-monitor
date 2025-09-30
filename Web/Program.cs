using Core;
using Core.Config;
using Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using Service;
using Web.Filters;
using Web.Middlewares;

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
        services.AddDbContext<Db>(optionsBuilder => {
            // 在这里配置，同时配合Db(DbContextOptions options)构造器，和DbContextOptionsBuilder.IsConfigured判断
            // 可以防止 Db.OnConfiguring() 中重复配置
            Db.Configure(optionsBuilder);
        });

        // Core 中 服务类
        services.AddCoreServices();

        // Service 中 服务类
        services.AddLocalServices();

        services.Configure<CookiePolicyOptions>(options => {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => false;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddScoped<WebSessionTimeoutResetActionFilter>();
        services.AddScoped<ExceptionFilter>();

        var mvcBuilder = services.AddControllersWithViews()
            .AddMvcOptions(options => {
                options.Filters.AddService<WebSessionTimeoutResetActionFilter>();
                options.Filters.AddService<ExceptionFilter>();
            })
            .AddNewtonsoftJson(options => {
                // options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // 这个ContractResolver【不会】改变属性大小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 这个ContractResolver【会】改变属性大小写
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
            });
        
#if DEBUG
        mvcBuilder.AddRazorRuntimeCompilation();
#endif

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