using System.Security.Cryptography;
using System.Text;

namespace Client
{
    public static class HashUtil
    {
        public static string GetMD5(string clearText)
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