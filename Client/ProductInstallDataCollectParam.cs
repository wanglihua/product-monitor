using Newtonsoft.Json;

namespace Client
{
    public class ProductInstallDataCollectParam
    {
        public static string DateTimeFormat => "yyyy-MM-dd HH:mm:ss";

        public static string AccountTypeAdmin => "管理员";
        public static string AccountTypeTeacher => "教师";
        public static string AccountTypeStudent => "学生";

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

        [JsonProperty("cpuSerialNo")]
        public string CpuSerialNo { get; set; } = "";

        [JsonProperty("netMacAddress")]
        public string NetMacAddress { get; set; } = "";

        [JsonProperty("sign")]
        public string Sign { get; set; } = "";
    }
}