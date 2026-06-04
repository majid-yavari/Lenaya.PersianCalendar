namespace Lenaya.PersianCalendar;

public readonly partial struct PersianDateTime
{

    /// <summary>Converts this instance to the equivalent <see cref="DateTime"/> value.</summary>
    /// <returns>A <see cref="DateTime"/> representing the same date and time as this instance.</returns>
    public DateTime ToDateTime() =>
        DateTime.SpecifyKind(
            PersianCalendarHelper.ToGregorian(Year, Month, Day, Hour, Minute, Second, Millisecond),
            Kind);

    /// <summary>Converts this instance to a SQLite julianday value.</summary>
    /// <returns>The SQLite julianday value representing this date and time.</returns>
    public double ToSqliteJulianDay()
    {
        var dateTime = ToDateTime();
        return SqliteJulianDayUnixEpoch + (dateTime.Ticks - DateTime.UnixEpoch.Ticks) / (double)TimeSpan.TicksPerDay;
    }

    /// <summary>Creates a <see cref="PersianDateTime"/> from a SQLite julianday value.</summary>
    /// <param name="sqliteJulianDay">The SQLite julianday value.</param>
    /// <param name="kind">The <see cref="DateTimeKind"/> to assign to the resulting date/time.</param>
    /// <returns>A <see cref="PersianDateTime"/> converted from the SQLite julianday value.</returns>
    public static PersianDateTime FromSqliteJulianDay(double sqliteJulianDay, DateTimeKind kind = DateTimeKind.Unspecified) =>
        new(sqliteJulianDay, kind);

    /// <summary>Converts this instance to a UTC <see cref="DateTime"/> value.</summary>
    /// <returns>A <see cref="DateTime"/> in UTC representing the same point in time.</returns>
    public DateTime ToUtcDateTime()
    {
        var dateTime = ToDateTime();
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            _ => dateTime.ToUniversalTime()
        };
    }

    /// <summary>Converts this instance to a local <see cref="DateTime"/> value.</summary>
    /// <returns>A <see cref="DateTime"/> in local time representing the same point in time.</returns>
    public DateTime ToLocalDateTime()
    {
        var dateTime = ToDateTime();
        return dateTime.Kind switch
        {
            DateTimeKind.Local => dateTime,
            _ => dateTime.ToLocalTime()
        };
    }

    /// <summary>Returns a <see cref="PersianDateTime"/> converted to Coordinated Universal Time (UTC).</summary>
    /// <returns>A new <see cref="PersianDateTime"/> with Kind set to <see cref="DateTimeKind.Utc"/> and values adjusted to UTC.</returns>
    public PersianDateTime ToUtc()
    {
        var utcDateTime = ToUtcDateTime();
        var (year, month, day) = PersianCalendarHelper.ToPersian(utcDateTime);
        return new PersianDateTime(year, month, day,
            utcDateTime.Hour, utcDateTime.Minute, utcDateTime.Second, utcDateTime.Millisecond,
            DateTimeKind.Utc);
    }

    /// <summary>Returns a <see cref="PersianDateTime"/> converted to the local time zone.</summary>
    /// <returns>A new <see cref="PersianDateTime"/> with Kind set to <see cref="DateTimeKind.Local"/> and values adjusted to local time.</returns>
    public PersianDateTime ToLocal()
    {
        var localDateTime = ToLocalDateTime();
        var (year, month, day) = PersianCalendarHelper.ToPersian(localDateTime);
        return new PersianDateTime(year, month, day,
            localDateTime.Hour, localDateTime.Minute, localDateTime.Second, localDateTime.Millisecond,
            DateTimeKind.Local);
    }


    /// <summary>Converts this instance to a Unix timestamp (seconds since 1970-01-01 UTC).</summary>
    /// <returns>The number of seconds since 1970-01-01T00:00:00Z.</returns>
    public long ToUnixTimestamp()
    {
        var utc = ToUtcDateTime();
        return new DateTimeOffset(utc).ToUnixTimeSeconds();
    }

    /// <summary>Converts this instance to a Unix timestamp (milliseconds since 1970-01-01 UTC).</summary>
    /// <returns>The number of milliseconds since 1970-01-01T00:00:00Z.</returns>
    public long ToUnixTimestampMilliseconds()
    {
        var utc = ToUtcDateTime();
        return new DateTimeOffset(utc).ToUnixTimeMilliseconds();
    }

    /// <summary>Creates a <see cref="PersianDateTime"/> from a Unix timestamp (seconds since 1970-01-01 UTC).</summary>
    /// <param name="seconds">The Unix timestamp in seconds.</param>
    /// <returns>A <see cref="PersianDateTime"/> representing the same point in time.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the timestamp is outside the valid range.</exception>
    public static PersianDateTime FromUnixTimestamp(long seconds)
    {
        var dateTime = DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
        return new PersianDateTime(dateTime);
    }

    /// <summary>Creates a <see cref="PersianDateTime"/> from a Unix timestamp (milliseconds since 1970-01-01 UTC).</summary>
    /// <param name="milliseconds">The Unix timestamp in milliseconds.</param>
    /// <returns>A <see cref="PersianDateTime"/> representing the same point in time.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the timestamp is outside the valid range.</exception>
    public static PersianDateTime FromUnixTimestampMilliseconds(long milliseconds)
    {
        var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
        return new PersianDateTime(dateTime);
    }

    /// <summary>Tries to parse a Persian date/time string into a <see cref="PersianDateTime"/>.</summary>
    /// <param name="s">The string to parse. Supports formats like "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", compact digits, and Persian digits.</param>
    /// <param name="result">When this method returns, contains the parsed <see cref="PersianDateTime"/> if successful, or default otherwise.</param>
    /// <returns><c>true</c> if the parse was successful; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (PersianDateTime.TryParse("1400/01/01", out var date))
    /// {
    ///     Console.WriteLine(date); // 1400/01/01
    /// }
    /// </code>
    /// </example>
    public static bool TryParse(string? s, out PersianDateTime result) =>
        PersianDateTimeParser.TryParse(s, out result);

    /// <summary>Tries to parse a Persian date/time string into a <see cref="PersianDateTime"/> with the specified <see cref="DateTimeKind"/>.</summary>
    /// <param name="s">The string to parse. Non-digit separators are ignored when the remaining digits form yyyyMMdd or yyyyMMddHHmmss.</param>
    /// <param name="kind">The <see cref="DateTimeKind"/> value to assign to the parsed value.</param>
    /// <param name="result">When this method returns, contains the parsed <see cref="PersianDateTime"/> if successful, or default otherwise.</param>
    /// <returns><c>true</c> if the parse was successful; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string? s, DateTimeKind kind, out PersianDateTime result) =>
        PersianDateTimeParser.TryParse(s, kind, out result);


    private static DateTime FromSqliteJulianDayToDateTime(double sqliteJulianDay, DateTimeKind kind)
    {
        ValidateKind(kind);
        if (double.IsNaN(sqliteJulianDay) || double.IsInfinity(sqliteJulianDay))
        {
            throw new ArgumentOutOfRangeException(nameof(sqliteJulianDay));
        }

        var milliseconds = checked((long)Math.Round((sqliteJulianDay - SqliteJulianDayUnixEpoch) * MillisecondsPerDay));
        var ticks = checked(milliseconds * TimeSpan.TicksPerMillisecond + DateTime.UnixEpoch.Ticks);
        if (ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks)
        {
            throw new ArgumentOutOfRangeException(nameof(sqliteJulianDay));
        }

        return new DateTime(ticks, kind);
    }
}
