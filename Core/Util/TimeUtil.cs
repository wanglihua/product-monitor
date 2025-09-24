namespace Core.Util;

public static class TimeUtil
{
    /// <summary>
    /// 等同于 Java 里面的 System.currentTimeMillis()
    /// </summary>
    public static long CurrentTimeMillis()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }

    /// <summary>
    /// 等同于 Java 里面的 System.currentTimeMillis() / 1000
    /// </summary>
    public static long CurrentTimeSeconds()
    {
        return CurrentTimeMillis() / 1000;
    }
}