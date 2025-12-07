using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Client
{
    public class ClientKeyService
    {
        private string salt => "dc2cfff6c7576a2bbbf4d85bd0c58fb6"; // 跟服务器上的值一致 

        /// <summary>
        /// 验证Key格式是否正确，这个算法跟服务器端算法一致
        /// </summary>
        public bool ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key) == true)
            {
                return false;
            }

            if (key.Length != 64) // 按算法肯定是长度 64
            {
                return false;
            }

            string guidStr = key.Substring(0, 32);
            if (Guid.TryParseExact(guidStr, "N", out Guid guid) == false)
            {
                return false;
            }

            var guidPartList = guid.ToString("D").Split('-').ToList();
            string hash = GetMD5(string.Concat(guidPartList.OrderByDescending(e => e)) + salt);

            return hash == key.Substring(32, 32);
        }

        private string GetMD5(string clearText)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hashData = Encoding.UTF8.GetBytes(clearText);
                hashData = md5.ComputeHash(hashData);

                var sb = new StringBuilder();
                foreach (byte b in hashData)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}