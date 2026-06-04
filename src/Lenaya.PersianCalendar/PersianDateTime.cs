namespace Lenaya.PersianCalendar;

/// <summary>
/// Represents a date and time in the Persian (Solar Hijri) calendar system.
/// Provides functionality for persian date/time arithmetic, formatting, and conversion.
/// </summary>
public readonly partial struct PersianDateTime : IComparable<PersianDateTime>, IComparable, IEquatable<PersianDateTime>, IFormattable
{
    private const double SqliteJulianDayUnixEpoch = 2440587.5;
    private const double MillisecondsPerDay = 86_400_000d;

    /// <summary>Gets the year component (1-9999) of the date represented by this instance.</summary>
    public int Year { get; }
    /// <summary>Gets the month component (1-12) of the date represented by this instance.</summary>
    public int Month { get; }
    /// <summary>Gets the day component (1-31) of the date represented by this instance.</summary>
    public int Day { get; }
    /// <summary>Gets the hour component (0-23) of the date represented by this instance.</summary>
    public int Hour { get; }
    /// <summary>Gets the minute component (0-59) of the date represented by this instance.</summary>
    public int Minute { get; }
    /// <summary>Gets the second component (0-59) of the date represented by this instance.</summary>
    public int Second { get; }
    /// <summary>Gets the millisecond component (0-999) of the date represented by this instance.</summary>
    public int Millisecond { get; }
    /// <summary>
    /// Gets the <see cref="DateTimeKind"/> value that indicates whether the time represented by this instance
    /// is based on local time, Coordinated Universal Time (UTC), or neither.
    /// </summary>
    public DateTimeKind Kind { get; }

    /// <summary>Gets the day of the year (1-366) represented by this instance.</summary>
    public int DayOfYear
    {
        get
        {
            var days = 0;
            for (var month = 1; month < Month; month++)
            {
                days += PersianCalendarHelper.DaysInMonth(Year, month);
            }

            return days + Day;
        }
    }

    /// <summary>Gets the week number of the year (1-53) based on the Persian calendar week starting on Saturday.</summary>
    public int WeekNumber => GetWeekOfYear();

    /// <summary>Gets the time-of-day component of this instance as a <see cref="TimeSpan"/>.</summary>
    public TimeSpan TimeOfDay => new(0, Hour, Minute, Second, Millisecond);

    /// <summary>Initializes a new instance of <see cref="PersianDateTime"/> with the specified date and time set to midnight.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <param name="month">The Persian month (1-12).</param>
    /// <param name="day">The Persian day (1-31).</param>
    /// <param name="kind">The <see cref="DateTimeKind"/> value (Unspecified, Utc, or Local). Defaults to Unspecified.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date is invalid.</exception>
    public PersianDateTime(int year, int month, int day, DateTimeKind kind = DateTimeKind.Unspecified)
        : this(year, month, day, 0, 0, 0, 0, kind)
    {
    }

    /// <summary>Initializes a new instance of <see cref="PersianDateTime"/> with the specified date, time, and kind set to Unspecified.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <param name="month">The Persian month (1-12).</param>
    /// <param name="day">The Persian day (1-31).</param>
    /// <param name="hour">The hour (0-23).</param>
    /// <param name="minute">The minute (0-59).</param>
    /// <param name="second">The second (0-59).</param>
    /// <param name="millisecond">The millisecond (0-999). Defaults to 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date or time is invalid.</exception>
    public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond = 0)
        : this(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified)
    {
    }

    /// <summary>Initializes a new instance of <see cref="PersianDateTime"/> with the specified date, time, and kind.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <param name="month">The Persian month (1-12).</param>
    /// <param name="day">The Persian day (1-31).</param>
    /// <param name="hour">The hour (0-23).</param>
    /// <param name="minute">The minute (0-59).</param>
    /// <param name="second">The second (0-59).</param>
    /// <param name="kind">The <see cref="DateTimeKind"/> value (Unspecified, Utc, or Local).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date, time, or kind is invalid.</exception>
    public PersianDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
        : this(year, month, day, hour, minute, second, 0, kind)
    {
    }

    /// <summary>Initializes a new instance of <see cref="PersianDateTime"/> with the specified date, time, and <see cref="DateTimeKind"/>.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <param name="month">The Persian month (1-12).</param>
    /// <param name="day">The Persian day (1-31).</param>
    /// <param name="hour">The hour (0-23).</param>
    /// <param name="minute">The minute (0-59).</param>
    /// <param name="second">The second (0-59).</param>
    /// <param name="millisecond">The millisecond (0-999).</param>
    /// <param name="kind">The <see cref="DateTimeKind"/> value (Unspecified, Utc, or Local).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date, time, or kind is invalid.</exception>
    public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
    {
        PersianCalendarHelper.ValidateDate(year, month, day);
        PersianCalendarHelper.ValidateTime(hour, minute, second, millisecond);
        ValidateKind(kind);

        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
        Millisecond = millisecond;
        Kind = kind;
    }

    /// <summary>Initializes a new instance of <see cref="PersianDateTime"/> by converting the specified <see cref="DateTime"/>.</summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to convert from.</param>
    /// <param name="kind">The kind to use when <paramref name="dateTime"/> is Unspecified. If <paramref name="dateTime"/> is Local or Utc, its existing kind is preserved.</param>
    public PersianDateTime(DateTime dateTime, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        var effectiveKind = dateTime.Kind == DateTimeKind.Unspecified ? kind : dateTime.Kind;
        ValidateKind(effectiveKind);

        var (year, month, day) = PersianCalendarHelper.ToPersian(dateTime);
        Year = year;
        Month = month;
        Day = day;
        Hour = dateTime.Hour;
        Minute = dateTime.Minute;
        Second = dateTime.Second;
        Millisecond = dateTime.Millisecond;
        Kind = effectiveKind;
    }

    /// <summary>Initializes a new instance by parsing a Persian date/time string.</summary>
    /// <param name="value">The string to parse. Non-digit characters are ignored, so values like "1405/11/13 22:13:45" are accepted.</param>
    /// <param name="kind">The <see cref="DateTimeKind"/> value to assign to the parsed value. Defaults to <see cref="DateTimeKind.Unspecified"/>.</param>
    /// <exception cref="FormatException">Thrown when the value cannot be parsed.</exception>
    public PersianDateTime(string value, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        ValidateKind(kind);
        if (!PersianDateTimeParser.TryParse(value, kind, out var parsed))
        {
            throw new FormatException("Invalid PersianDateTime format.");
        }

        this = parsed;
    }

    /// <summary>Initializes a new instance from a SQLite julianday value.</summary>
    /// <param name="sqliteJulianDay">The SQLite julianday value.</param>
    /// <param name="kind">The <see cref="DateTimeKind"/> to assign to the resulting date/time.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside the valid <see cref="DateTime"/> range.</exception>
    public PersianDateTime(double sqliteJulianDay, DateTimeKind kind = DateTimeKind.Unspecified)
        : this(FromSqliteJulianDayToDateTime(sqliteJulianDay, kind))
    {
    }


    private static void ValidateKind(DateTimeKind kind)
    {
        if (kind is not DateTimeKind.Unspecified and not DateTimeKind.Utc and not DateTimeKind.Local)
        {
            throw new ArgumentOutOfRangeException(nameof(kind));
        }
    }
}
