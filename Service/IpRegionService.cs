using Core;
using Newtonsoft.Json.Linq;
using NLog;

namespace Service;

[Service(ServiceType.Scoped)]
public class IpRegionService
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public string GetIpAddressProvince(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress) == true)
        {
            return "";
        }

        try
        {
            var httpClient = new HttpClient();
            string url = $"https://ip9.com.cn/get?ip={ipAddress}";
            var result = httpClient.GetAsync(url).GetAwaiter().GetResult();
            if (result.IsSuccessStatusCode)
            {
                string response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var jObject = JObject.Parse(response);
                string province = jObject.SelectToken("data.prov")?.ToString() ?? "";
                return province;
            }

            return "";
        }
        catch (Exception exp)
        {
            log.Error(exp);
            return "";
        }
    }
}