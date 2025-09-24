using System.Reflection;
using Core.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Service;

public static class ServiceExtensions
{
    public static void AddLocalServices(this IServiceCollection services)
    {
        Assembly assembly = typeof(Dummy).Assembly;
        ServiceUtil.AddAssemblyService(services, assembly);
    }
}