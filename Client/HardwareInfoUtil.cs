using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Client
{
    public static class HardwareInfoUtil
    {
        public static string GetMacAddress()
        {
            var moAddress = "";

            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            var moAddressList = new List<string>();

            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            if (managementObjectCollection == null)
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

        public static string GetCpuId()
        {
            string cpuId = "";

            ManagementClass managementClass = new ManagementClass("Win32_Processor");

            var cupInfoList = new List<string>();

            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            if (managementObjectCollection == null)
            {
                return "";
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

            return cpuId;
        }
    }
}