namespace Lenaya.PersianCalendar;

public readonly partial struct PersianDateTime
{

    /// <summary>Converts this instance to its equivalent short date string representation.</summary>
    /// <returns>A string in the format "yyyy/MM/dd HH:mm:ss".</returns>
    public override string ToString() => ToShortDateTimeString();

    /// <summary>Converts this instance to its equivalent string representation using the specified format.</summary>
    /// <param name="format">The format string. Supports standard and custom Persian date/time format specifiers.</param>
    /// <returns>A string representation of this instance as specified by <paramref name="format"/>.</returns>
    /// <example>
    /// <code>
    /// var date = new PersianDateTime(1400, 7, 15, 10, 30, 0);
    /// string s = date.ToString("yyyy/MM/dd HH:mm"); // "1400/07/15 10:30"
    /// </code>
    /// </example>
    /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
    public string ToString(string? format) =>
        PersianDateTimeFormatter.Format(this, format);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) =>
        ToString(format);

    /// <summary>Converts this instance to its short date string representation (yyyy/MM/dd).</summary>
    /// <returns>A short date string representation of this instance.</returns>
    public string ToShortDateString() =>
        PersianDateTimeFormatter.ToShortDateString(this);

    /// <summary>Converts this instance to its short date-time string representation (yyyy/MM/dd HH:mm:ss).</summary>
    /// <returns>A short date-time string representation of this instance.</returns>
    public string ToShortDateTimeString() =>
        PersianDateTimeFormatter.ToShortDateTimeString(this);

    /// <summary>Converts this instance to a 14-digit numeric string (yyyyMMddHHmmss).</summary>
    /// <returns>A 14-digit string representation of this instance.</returns>
    public string To14DigitString() =>
        PersianDateTimeFormatter.To14DigitString(this);

    /// <summary>Converts this instance to an 8-digit numeric date string (yyyyMMdd).</summary>
    /// <returns>An 8-digit date string representation of this instance.</returns>
    public string To8DigitString() =>
        PersianDateTimeFormatter.To8DigitString(this);

    /// <summary>Converts this instance to its long date string representation using standard month names and Solar Hijri era.</summary>
    /// <returns>A long date string representation of this instance.</returns>
    public string ToLongDateString() =>
        ToLongDateString(MonthNameStyle.Standard, PersianCalendarEra.SolarHijri);

    /// <summary>Converts this instance to its long date string representation using the specified month name style and era.</summary>
    /// <param name="style">The style of month names to use (Standard or Astronomical).</param>
    /// <param name="era">The Persian calendar era (SolarHijri or Shahanshahi).</param>
    /// <returns>A long date string representation of this instance.</returns>
    public string ToLongDateString(MonthNameStyle style, PersianCalendarEra era = PersianCalendarEra.SolarHijri) =>
        PersianDateTimeFormatter.ToLongDateString(this, style, era);

    /// <summary>Converts this instance to its long date-time string representation using standard month names.</summary>
    /// <returns>A long date-time string representation of this instance.</returns>
    public string ToLongDateTimeString() =>
        PersianDateTimeFormatter.ToLongDateTimeString(this);

    /// <summary>Converts this instance to its long date-time string representation using the specified month name style and era.</summary>
    /// <param name="style">The style of month names to use (Standard or Astronomical).</param>
    /// <param name="era">The Persian calendar era (SolarHijri or Shahanshahi).</param>
    /// <returns>A long date-time string representation of this instance.</returns>
    public string ToLongDateTimeString(MonthNameStyle style, PersianCalendarEra era = PersianCalendarEra.SolarHijri) =>
        PersianDateTimeFormatter.ToLongDateTimeString(this, style, era);

    /// <summary>Gets the standard Persian month name for the specified month number.</summary>
    /// <param name="month">The month number (1-12).</param>
    /// <returns>The standard Persian month name (e.g., "فروردین" for month 1).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="month"/> is not between 1 and 12.</exception>
    public static string GetStandardMonthName(int month) =>
        PersianDateTimeFormatter.GetStandardMonthName(month);

    /// <summary>Gets the astronomical Persian month name for the specified month number.</summary>
    /// <param name="month">The month number (1-12).</param>
    /// <returns>The astronomical Persian month name (e.g., "حمل" for month 1).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="month"/> is not between 1 and 12.</exception>
    public static string GetAstronomicalMonthName(int month) =>
        PersianDateTimeFormatter.GetAstronomicalMonthName(month);

    /// <summary>Returns the year in the specified Persian calendar era.</summary>
    /// <param name="era">The Persian calendar era (SolarHijri or Shahanshahi).</param>
    /// <returns>The year adjusted for the specified era.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="era"/> is not a valid <see cref="PersianCalendarEra"/>.</exception>
    public int YearEra(PersianCalendarEra era) => era switch
    {
        PersianCalendarEra.SolarHijri => Year,
        PersianCalendarEra.Shahanshahi => Year + 1180,
        _ => throw new ArgumentOutOfRangeException(nameof(era))
    };

    /// <summary>Returns the given year converted to the specified Persian calendar era.</summary>
    /// <param name="year">The Solar Hijri year.</param>
    /// <param name="era">The target Persian calendar era.</param>
    /// <returns>The year adjusted for the specified era.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="era"/> is not a valid <see cref="PersianCalendarEra"/>.</exception>
    public static int GetYearEra(int year, PersianCalendarEra era) => era switch
    {
        PersianCalendarEra.SolarHijri => year,
        PersianCalendarEra.Shahanshahi => year + 1180,
        _ => throw new ArgumentOutOfRangeException(nameof(era))
    };
}
