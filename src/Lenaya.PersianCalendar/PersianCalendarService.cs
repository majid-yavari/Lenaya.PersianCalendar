using Microsoft.Extensions.Options;

namespace Lenaya.PersianCalendar;

/// <summary>Default implementation of <see cref="IPersianCalendar"/> using the Persian (Solar Hijri) calendar system.</summary>
public class PersianCalendarService : IPersianCalendar
{
    private readonly PersianCalendarOptions _options;

    /// <summary>Initializes a new instance with options via dependency injection.</summary>
    /// <param name="options">The options instance.</param>
    public PersianCalendarService(IOptions<PersianCalendarOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>Initializes a new instance with default options.</summary>
    public PersianCalendarService() : this(Options.Create(new PersianCalendarOptions()))
    {
    }

    /// <summary>Gets the current Persian date (local time, date component only).</summary>
    public PersianDateTime Today => PersianDateTime.Today;

    /// <summary>Gets the current Persian date and time (local time).</summary>
    public PersianDateTime Now => PersianDateTime.Now;

    /// <summary>Converts a <see cref="DateTime"/> value to a <see cref="PersianDateTime"/>.</summary>
    /// <param name="dateTime">The Gregorian date and time to convert.</param>
    /// <returns>A <see cref="PersianDateTime"/> representing the converted date.</returns>
    public PersianDateTime FromDateTime(DateTime dateTime) =>
        new(dateTime);

    /// <summary>Converts a <see cref="PersianDateTime"/> value to a <see cref="DateTime"/>.</summary>
    /// <param name="date">The Persian date and time to convert.</param>
    /// <returns>A <see cref="DateTime"/> representing the converted Gregorian date.</returns>
    public DateTime ToDateTime(PersianDateTime date) =>
        date.ToDateTime();

    /// <summary>Determines whether the specified Persian year is a leap year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <returns><c>true</c> if the year is a leap year; otherwise, <c>false</c>.</returns>
    public bool IsLeapYear(int year) =>
        PersianDateTime.IsLeapYear(year);

    /// <summary>Returns the number of days in the specified Persian month and year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <param name="month">The Persian month (1–12).</param>
    /// <returns>The number of days in the specified month.</returns>
    public int DaysInMonth(int year, int month) =>
        PersianDateTime.DaysInMonth(year, month);

    /// <summary>Returns the number of days in the specified Persian year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <returns>The number of days in the specified year.</returns>
    public int DaysInYear(int year) =>
        PersianDateTime.DaysInYear(year);

    /// <summary>Validates whether the specified Persian date is valid.</summary>
    /// <param name="year">The Persian year.</param>
    /// <param name="month">The Persian month.</param>
    /// <param name="day">The Persian day.</param>
    /// <returns><c>true</c> if the date is valid; otherwise, <c>false</c>.</returns>
    public bool IsValidDate(int year, int month, int day) =>
        PersianDateTime.IsValidDate(year, month, day);

    /// <summary>Formats the specified Persian date using configured options (month name style and era).</summary>
    /// <param name="date">The Persian date to format.</param>
    /// <returns>A string representation of the date.</returns>
    public string Format(PersianDateTime date)
    {
        return HasTimeComponent(date)
            ? date.ToLongDateTimeString(_options.MonthNameStyle, _options.Era)
            : date.ToLongDateString(_options.MonthNameStyle, _options.Era);
    }

    /// <summary>Formats the specified Persian date using a custom format string.</summary>
    /// <param name="date">The Persian date to format.</param>
    /// <param name="format">The custom format string.</param>
    /// <returns>A string representation of the date.</returns>
    public string Format(PersianDateTime date, string format) =>
        date.ToString(format);

    private static bool HasTimeComponent(PersianDateTime date) =>
        date.Hour != 0 || date.Minute != 0 || date.Second != 0 || date.Millisecond != 0;
}
