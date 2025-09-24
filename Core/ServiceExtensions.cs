using System.Reflection;
using Core.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceExtensions
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        Assembly assembly = typeof(Mock).Assembly;
        ServiceUtil.AddAssemblyService(services, assembly);
    }
}
