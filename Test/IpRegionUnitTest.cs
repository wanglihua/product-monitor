using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Test;

[TestFixture]
public class IpProvinceUnitTest
{
    [Test]
    public void TestRegion()
    {
        var httpClient = new HttpClient();
        string url = $"https://ip9.com.cn/get?ip=58.30.0.0";
        var result = httpClient.GetAsync(url).GetAwaiter().GetResult();
        if (result.IsSuccessStatusCode)
        {
            string response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine(response);

            JObject jObject = JObject.Parse(response);
            string province = jObject.SelectToken("data.prov")?.ToString() ?? "";
            Console.WriteLine(province);
        }
    }
}