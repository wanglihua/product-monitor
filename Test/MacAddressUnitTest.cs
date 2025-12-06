using System.Management;
using NUnit.Framework;

namespace Test;

#pragma warning disable CA1416

[TestFixture]
public class MacAddressUnitTest
{
    private string GetMacAddress()
    {
        var moAddress = "";

        ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

        var moAddressList = new List<string>();

        ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

        if (managementObjectCollection is null)
        {
            return "";
        }

        foreach (var managementObject in managementObjectCollection)
        {
            if ((bool)managementObject["IPEnabled"] == true)
            {
                var moAddressStr = managementObject["MacAddress"].ToString() ?? "";
                moAddressList.Add(moAddressStr);
            }

            managementObject.Dispose();
        }

        if (moAddressList.Any())
        {
            moAddress = moAddressList.OrderByDescending(m => m).FirstOrDefault() ?? "";
        }

        return moAddress.Replace(":", "") + "006";
    }

    [Test]
    public void TestGetMacAddress()
    {
        string macAddress = GetMacAddress();
        Console.WriteLine(macAddress);
    }
}