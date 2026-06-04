using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lenaya.PersianCalendar;

/// <summary>
/// Converts a <see cref="PersianDateTime"/> to and from JSON using a 14-digit string format.
/// </summary>
public class PersianDateTimeJsonConverter : JsonConverter<PersianDateTime>
{
    /// <summary>
    /// Reads and converts JSON to a <see cref="PersianDateTime"/>.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>A <see cref="PersianDateTime"/> parsed from the JSON string.</returns>
    /// <exception cref="JsonException">Thrown when the JSON token is not a valid PersianDateTime string.</exception>
    public override PersianDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var str = reader.GetString();
            if (str is not null && PersianDateTime.TryParse(str, out var date))
            {
                return date;
            }
        }
        throw new JsonException("Invalid PersianDateTime format.");
    }

    /// <summary>
    /// Writes a <see cref="PersianDateTime"/> as a JSON string using its 14-digit representation.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer.</param>
    /// <param name="value">The PersianDateTime value to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, PersianDateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.To14DigitString());
    }
}
