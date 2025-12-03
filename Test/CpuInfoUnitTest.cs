using System.Management;
using NUnit.Framework;

namespace Test;

#pragma warning disable CA1416

[TestFixture]
public class CpuInfoUnitTest
{
    [Test]
    public void GetCpuInfo()
    {
        string cpuId = "";

        ManagementClass managementClass = new ManagementClass("Win32_Processor");

        var cupInfoList = new List<string>();

        ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

        if (managementObjectCollection == null)
        {
            Console.WriteLine("");
            return;
        }

        foreach (var managementObject in managementObjectCollection)
        {
            if (managementObject?.Properties["ProcessorId"]?.Value != null)
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

        Console.WriteLine(cpuId);
    }
}