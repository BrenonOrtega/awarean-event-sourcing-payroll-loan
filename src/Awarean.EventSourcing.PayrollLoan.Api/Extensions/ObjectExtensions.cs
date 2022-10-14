using System.Text.Json;
using System.Text.Json.Serialization;

namespace Awarean.EventSourcing.PayrollLoans.Api.Extensions;
public static class ObjectExtensions
{
    public static string AsJson(this object obj) => JsonSerializer.Serialize(obj, _options);
    public static T FromJson<T>(this string @string)
    {
        if(string.IsNullOrEmpty(@string) || string.IsNullOrWhiteSpace(@string))
            return default(T);

        var serialized = JsonSerializer.Deserialize<T>(@string, _options);

        return serialized;
    }

    private static readonly JsonSerializerOptions _options = new() 
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
    };
}
