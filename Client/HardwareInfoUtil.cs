using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Client
{
    public static class HardwareInfoUtil
    {
        private static string macAddressCache = "";
        private static string cpuIdCache = "";

        public static string GetMacAddress()
        {
            // 获取成功过了就不用第二次获取了
            if (string.IsNullOrWhiteSpace(macAddressCache) == false)
            {
                return macAddressCache;
            }

            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");

            var moAddressList = new List<string>();

            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            // 这个情况一般不会发生
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

            string macAddress = "";
            if (moAddressList.Any())
            {
                macAddress = moAddressList.OrderByDescending(m => m).FirstOrDefault() ?? "";
            }

            macAddressCache = macAddress.Replace(":", "") + "006";

            return macAddressCache;
        }

        public static string GetCpuId()
        {
            // 获取过成功了就不用第二次获取了
            if (string.IsNullOrWhiteSpace(cpuIdCache) == false)
            {
                return cpuIdCache;
            }

            ManagementClass managementClass = new ManagementClass("Win32_Processor");

            var cupInfoList = new List<string>();

            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();

            // 这个情况一般不会发生
            if (managementObjectCollection == null)
            {
                return "";
            }

            string cpuId = "";
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

            cpuIdCache = cpuId;

            return cpuIdCache;
        }
    }
}