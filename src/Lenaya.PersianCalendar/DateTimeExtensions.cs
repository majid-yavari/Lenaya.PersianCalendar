namespace Lenaya.PersianCalendar;

/// <summary>Provides extension methods for converting <see cref="DateTime"/> values to and from Unix timestamps.</summary>
public static class DateTimeExtensions
{
    /// <summary>Converts a <see cref="DateTime"/> to a Unix timestamp (seconds since 1970-01-01).</summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <returns>The number of seconds since Unix epoch.</returns>
    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        var utc = dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
        return new DateTimeOffset(utc).ToUnixTimeSeconds();
    }

    /// <summary>Converts a <see cref="DateTime"/> to a Unix timestamp in milliseconds.</summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <returns>The number of milliseconds since Unix epoch.</returns>
    public static long ToUnixTimestampMilliseconds(this DateTime dateTime)
    {
        var utc = dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
        return new DateTimeOffset(utc).ToUnixTimeMilliseconds();
    }

    /// <summary>Creates a <see cref="DateTime"/> from a Unix timestamp (seconds).</summary>
    /// <param name="seconds">The number of seconds since Unix epoch.</param>
    /// <returns>The corresponding UTC <see cref="DateTime"/>.</returns>
    public static DateTime FromUnixTimestamp(long seconds) =>
        DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;

    /// <summary>Creates a <see cref="DateTime"/> from a Unix timestamp in milliseconds.</summary>
    /// <param name="milliseconds">The number of milliseconds since Unix epoch.</param>
    /// <returns>The corresponding UTC <see cref="DateTime"/>.</returns>
    public static DateTime FromUnixTimestampMilliseconds(long milliseconds) =>
        DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
}
