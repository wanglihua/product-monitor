namespace Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceAttribute : Attribute
{
    public ServiceType ServiceType { get; set; }

    public ServiceAttribute(ServiceType serviceType)
    {
        this.ServiceType = serviceType;
    }
}