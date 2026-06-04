using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lenaya.PersianCalendar;

/// <summary>
/// Converts a <see cref="PersianDateTime"/> to and from JSON object form while preserving milliseconds and <see cref="DateTimeKind"/>.
/// </summary>
public class PersianDateTimeFullJsonConverter : JsonConverter<PersianDateTime>
{
    /// <summary>
    /// Reads and converts JSON object data to a <see cref="PersianDateTime"/>.
    /// </summary>
    /// <param name="reader">The UTF-8 JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>A <see cref="PersianDateTime"/> parsed from the JSON object.</returns>
    /// <exception cref="JsonException">Thrown when the JSON object is missing required fields or contains invalid values.</exception>
    public override PersianDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a PersianDateTime JSON object.");
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        try
        {
            var year = GetRequiredInt32(root, "year");
            var month = GetRequiredInt32(root, "month");
            var day = GetRequiredInt32(root, "day");
            var hour = GetOptionalInt32(root, "hour");
            var minute = GetOptionalInt32(root, "minute");
            var second = GetOptionalInt32(root, "second");
            var millisecond = GetOptionalInt32(root, "millisecond");
            var kind = GetOptionalKind(root);

            return new PersianDateTime(year, month, day, hour, minute, second, millisecond, kind);
        }
        catch (Exception ex) when (ex is ArgumentOutOfRangeException or FormatException or InvalidOperationException)
        {
            throw new JsonException("Invalid PersianDateTime object.", ex);
        }
    }

    /// <summary>
    /// Writes a <see cref="PersianDateTime"/> as a JSON object preserving all date, time, and kind components.
    /// </summary>
    /// <param name="writer">The UTF-8 JSON writer.</param>
    /// <param name="value">The PersianDateTime value to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, PersianDateTime value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("year", value.Year);
        writer.WriteNumber("month", value.Month);
        writer.WriteNumber("day", value.Day);
        writer.WriteNumber("hour", value.Hour);
        writer.WriteNumber("minute", value.Minute);
        writer.WriteNumber("second", value.Second);
        writer.WriteNumber("millisecond", value.Millisecond);
        writer.WriteString("kind", value.Kind.ToString());
        writer.WriteEndObject();
    }

    private static int GetRequiredInt32(JsonElement root, string propertyName)
    {
        if (!root.TryGetProperty(propertyName, out var property))
        {
            throw new JsonException($"Missing required property '{propertyName}'.");
        }

        return property.GetInt32();
    }

    private static int GetOptionalInt32(JsonElement root, string propertyName) =>
        root.TryGetProperty(propertyName, out var property) ? property.GetInt32() : 0;

    private static DateTimeKind GetOptionalKind(JsonElement root)
    {
        if (!root.TryGetProperty("kind", out var property))
        {
            return DateTimeKind.Unspecified;
        }

        if (property.ValueKind != JsonValueKind.String ||
            !Enum.TryParse<DateTimeKind>(property.GetString(), ignoreCase: true, out var kind))
        {
            throw new JsonException("Invalid DateTimeKind value.");
        }

        return kind;
    }
}
