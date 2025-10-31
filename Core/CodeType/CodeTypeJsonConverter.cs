using Newtonsoft.Json;

namespace Core.CodeType;

/// <summary>
/// 用法 [JsonConverter(typeof(XXXCodeTypeJsonConverter))]
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class CodeTypeJsonConverter<T> : JsonConverter<T> where T : CodeTypeBase, ICodeType<T>
{
    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        throw new Exception($"转换器 {this.GetType().Name} 只用于读取JSON转换");
    }

    public override T ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string typeIdStr = reader.Value?.ToString() ?? string.Empty;

        if (int.TryParse(typeIdStr, out int typeId) == false)
        {
            return T.GetById(0);
        }

        return T.GetById(typeId);
    }
}