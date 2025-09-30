using Microsoft.Extensions.Caching.Memory;

namespace Core.CodeType;

using System.Reflection;

/// <summary>
/// 这是通过反射来获取 GetByCode 方法，
/// 如果是 .net7 或者以后，就可以 用 static virtual/abstract interface members 实现
/// </summary>
public static class CodeTypeUtil
{
    private static readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

    private const string GET_BY_CODE_METHOD_NAME = "GetByCode";

    public static T GetByCode<T>(int code) where T : CodeTypeBase
    {
        if (code < 0)
        {
            code = 0;
        }

        var typeOfT = typeof(T);

        string cacheKey = GetCacheKey(typeOfT, code);

        if (memoryCache.TryGetValue(cacheKey, out object? codeType) && codeType != null)
        {
            return (T)codeType;
        }

        var method = typeOfT.GetMethod(GET_BY_CODE_METHOD_NAME, BindingFlags.Static | BindingFlags.Public);
        if (method == null)
        {
            throw new Exception($"{typeof(T).Name} public static {GET_BY_CODE_METHOD_NAME}() 方法 不存在！");
        }

        var result = method.Invoke(null, new object[] { code }) as T;
        if (result == null)
        {
            throw new Exception($"{typeof(T).Name} public static {GET_BY_CODE_METHOD_NAME}() 返回为空或返回类型不匹配！");
        }

        if (result.Code <= 0)
        {
            cacheKey = GetCacheKey(typeOfT, 0);
        }

        memoryCache.Set(cacheKey, result);

        return result;
    }

    private static string GetCacheKey(Type typeOfT, int code)
    {
        return $"{typeOfT.FullName}_{code}";
    }
}