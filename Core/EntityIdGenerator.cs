namespace Core;

public static class EntityIdGenerator
{
    // timestamp 起始，从 2025.1.1 开始算起，用于减小 id 高位数值
    private static readonly long timestampBegin = new DateTimeOffset(new DateTime(2025, 1, 1)).ToUnixTimeMilliseconds();

    private static long lastTimestamp = -1L;
    private static long sequence = 0L;

    private static readonly object lockObj = new object();

    public static long NewId()
    {
        lock (lockObj)
        {
            long timestamp = GetTimestamp();

            if (timestamp != lastTimestamp)
            {
                sequence = 0L;
            }

            sequence++;

#if DEBUG
            // 去这么小的数值，只是为了方便测试和确认
            if (sequence >= 16) // 2 ^ 4 = 16
#else
            if (sequence >= 4096) // 2 ^ 12 = 4096
#endif
            {
                sequence = 1;

                while (timestamp <= lastTimestamp) // 实际上这个 <= 号，一般只会成立 = 号 
                {
                    timestamp = GetTimestamp();
                }
            }

            lastTimestamp = timestamp;

#if DEBUG
            return (timestamp << 4) | sequence;
#else
            return (timestamp << 12) | sequence;
#endif
        }
    }

    private static long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timestampBegin;
    }
}