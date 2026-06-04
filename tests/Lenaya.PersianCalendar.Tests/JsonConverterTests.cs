using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class JsonConverterTests
{
    [Fact]
    public void SerializeDeserialize_RoundTrip_ShouldPreserve()
    {
        var options = new System.Text.Json.JsonSerializerOptions();
        options.Converters.Add(new PersianDateTimeJsonConverter());

        var original = new PersianDateTime(1403, 6, 15, 10, 30, 0);
        var json = System.Text.Json.JsonSerializer.Serialize(original, options);
        var deserialized = System.Text.Json.JsonSerializer.Deserialize<PersianDateTime>(json, options);

        Assert.Equal(original, deserialized);
    }

    [Fact]
    public void FullConverter_RoundTrip_ShouldPreserveMillisecondsAndKind()
    {
        var options = new System.Text.Json.JsonSerializerOptions();
        options.Converters.Add(new PersianDateTimeFullJsonConverter());

        var original = new PersianDateTime(1403, 6, 15, 10, 30, 45, 123, DateTimeKind.Utc);
        var json = System.Text.Json.JsonSerializer.Serialize(original, options);
        var deserialized = System.Text.Json.JsonSerializer.Deserialize<PersianDateTime>(json, options);

        Assert.Equal(original, deserialized);
        Assert.Equal(123, deserialized.Millisecond);
        Assert.Equal(DateTimeKind.Utc, deserialized.Kind);
    }

    [Fact]
    public void FullConverter_ShouldReadObjectWithOptionalTimeFields()
    {
        var options = new System.Text.Json.JsonSerializerOptions();
        options.Converters.Add(new PersianDateTimeFullJsonConverter());

        const string json = """
                            {
                              "year": 1403,
                              "month": 1,
                              "day": 1,
                              "kind": "Local"
                            }
                            """;

        var result = System.Text.Json.JsonSerializer.Deserialize<PersianDateTime>(json, options);

        Assert.Equal(new PersianDateTime(1403, 1, 1, DateTimeKind.Local), result);
    }
}

