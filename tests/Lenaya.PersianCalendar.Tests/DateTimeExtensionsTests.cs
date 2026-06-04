using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class DateTimeExtensionsTests
{
    [Fact]
    public void ToUnixTimestamp_UtcDateTime_ShouldReturnCorrectValue()
    {
        var dt = new DateTime(2023, 5, 23, 0, 0, 0, DateTimeKind.Utc);
        var timestamp = dt.ToUnixTimestamp();
        var back = DateTimeExtensions.FromUnixTimestamp(timestamp);
        Assert.Equal(dt, back);
    }

    [Fact]
    public void ToUnixTimestampMilliseconds_UtcDateTime_ShouldReturnCorrectValue()
    {
        var dt = new DateTime(2023, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc);
        var timestamp = dt.ToUnixTimestampMilliseconds();
        var back = DateTimeExtensions.FromUnixTimestampMilliseconds(timestamp);
        Assert.Equal(dt, back);
    }
}

