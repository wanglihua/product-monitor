using System;
using System.Globalization;
using System.Text;

namespace Client
{
    public static class DataCollectUtil
    {
        private static string salt => "dc2cfff6c7576a2bbbf4d85bd0c58fb6"; // jcjy md5 
        private static TimeSpan validTimeSpan => TimeSpan.FromMinutes(10);

        public static string CalSign(ProductInstallDataCollectParam param)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(salt);
            sb.Append($"key={param.Key}");
            sb.Append($"accountType={param.AccountType}");
            sb.Append($"loginUser={param.LoginUser}");
            sb.Append($"useTime={param.UseTime}");
            sb.Append($"cpuSerialNo={param.CpuSerialNo}");
            sb.Append($"netMacAddress={param.NetMacAddress}");
            sb.Append($"sendTime={param.SendTime}");
            sb.Append(salt);

            return HashUtil.GetMD5(sb.ToString());
        }

        public static bool VerifySign(ProductInstallDataCollectParam param)
        {
            if (DateTime.TryParseExact(
                    param.SendTime,
                    ProductInstallDataCollectParam.DateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime sendTime) == false)
            {
                return false;
            }

            // 太久前发送的无效
            if ((DateTime.Now - sendTime) > validTimeSpan)
            {
                return false;
            }

            // 时效符合的话，检查签名
            string sign = CalSign(param);
            return sign == param.Sign;
        }

        public static string CalSign(ProductUrlDataCollectParam param)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(salt);
            sb.Append($"key={param.Key}");
            sb.Append($"accountType={param.AccountType}");
            sb.Append($"loginUser={param.LoginUser}");
            sb.Append($"password={param.Password}");
            sb.Append($"useTime={param.UseTime}");
            sb.Append($"sendTime={param.SendTime}");
            sb.Append(salt);

            return HashUtil.GetMD5(sb.ToString());
        }

        public static bool VerifySign(ProductUrlDataCollectParam param)
        {
            if (DateTime.TryParseExact(
                    param.SendTime,
                    ProductUrlDataCollectParam.DateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime sendTime) == false)
            {
                return false;
            }

            // 太久前发送的无效
            if ((DateTime.Now - sendTime) > validTimeSpan)
            {
                return false;
            }

            // 时效符合的话，检查签名
            string sign = CalSign(param);
            return sign == param.Sign;
        }
    }
}