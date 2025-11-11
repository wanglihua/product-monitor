using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Util;

public static class ServiceUtil
{
    public static void AddAssemblyService(IServiceCollection services, Assembly assembly)
    {
        IList<Type> typeList = assembly.GetTypes().Where(t => t.IsClass && (!t.IsAbstract)).ToList();
        foreach (Type type in typeList)
        {
            var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();
            if (serviceAttribute is not null)
            {
                if (serviceAttribute.ServiceType == ServiceType.Singleton)
                {
                    services.AddSingleton(type);
                }
                else if (serviceAttribute.ServiceType == ServiceType.Scoped)
                {
                    services.AddScoped(type);
                }
                else if (serviceAttribute.ServiceType == ServiceType.Transient)
                {
                    services.AddTransient(type);
                }
            }
        }
    }
}