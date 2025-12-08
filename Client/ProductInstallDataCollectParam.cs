using Newtonsoft.Json;

namespace Client
{
    public class ProductInstallDataCollectParam
    {
        [JsonProperty("key")]
        public string Key { get; set; } = "";

        [JsonProperty("useTime")]
        public string UseTime { get; set; } = "";

        [JsonProperty("accountType")]
        public string AccountType { get; set; } = "";

        [JsonProperty("loginUser")]
        public string LoginUser { get; set; } = "";

        [JsonProperty("sendTime")]
        public string SendTime { get; set; } = "";

        [JsonProperty("sign")]
        public string Sign { get; set; } = "";
    }
}