using System.Management;
using NUnit.Framework;

namespace Test;

#pragma warning disable CA1416

[TestFixture]
public class CpuInfoUnitTest
{
    private string GetCpuId()
    {
        string cpuId = "";

        ManagementClass managementClass = new ManagementClass("Win32_Processor");

        var cupInfoList = new List<string>();

        ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

        if (managementObjectCollection is null)
        {
            return "";
        }

        foreach (var managementObject in managementObjectCollection)
        {
            if (managementObject?.Properties["ProcessorId"]?.Value is not null)
            {
                var cpuIdStr = managementObject.Properties["ProcessorId"].Value.ToString() ?? "";
                cupInfoList.Add(cpuIdStr);
            }

            managementObject?.Dispose();
        }

        if (cupInfoList.Any())
        {
            cpuId = cupInfoList.OrderByDescending(m => m).FirstOrDefault() ?? "";
        }

        return cpuId;
    }

    [Test]
    public void TestGetCpuId()
    {
        string cpuId = GetCpuId();
        Console.WriteLine(cpuId);
    }
}