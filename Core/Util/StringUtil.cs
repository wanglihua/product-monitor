using System.Text;

namespace Core.Util;

public class StringUtil
{
    /// <summary>
    /// 如果字符串长度超过了限制值，就截取不超过的部分
    /// </summary>
    public static string LimitMaxLength(string str, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return String.Empty;
        }

        if (str.Length > maxLength)
        {
            str = str.Substring(0, maxLength);
        }

        return str;
    }

    public static string UnicodeToUtf8(string content)
    {
        return Encoding.UTF8.GetString(Encoding.Convert(
            Encoding.Unicode,
            Encoding.UTF8,
            Encoding.Unicode.GetBytes(content ?? string.Empty)
        ));
    }
}