using Newtonsoft.Json;

namespace Core.CodeType;

/// <summary>
/// 用法 [JsonConverter(typeof(XXXCodeTypeJsonConverter))]
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class CodeTypeJsonConverter<T> : JsonConverter<T> where T : CodeTypeBase
{
    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        throw new Exception($"转换器 {this.GetType().Name} 只用于读取JSON转换");
    }

    public override T ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string codeStr = reader.Value?.ToString() ?? string.Empty;

        if (int.TryParse(codeStr, out int code) == false)
        {
            return GetByCode(0);
        }

        return GetByCode(code);
    }

    // protected abstract T GetByCode(string code);

    private T GetByCode(int code)
    {
        return CodeTypeUtil.GetByCode<T>(code);
    }
}