namespace Lenaya.PersianCalendar;

/// <summary>Provides functionality to work with the Persian (Solar Hijri) calendar.</summary>
public interface IPersianCalendar
{
    /// <summary>Gets the current Persian date (local time, date component only).</summary>
    PersianDateTime Today { get; }

    /// <summary>Gets the current Persian date and time (local time).</summary>
    PersianDateTime Now { get; }

    /// <summary>Converts a <see cref="DateTime"/> value to a <see cref="PersianDateTime"/>.</summary>
    /// <param name="dateTime">The Gregorian date and time to convert.</param>
    /// <returns>A <see cref="PersianDateTime"/> representing the converted date.</returns>
    PersianDateTime FromDateTime(DateTime dateTime);

    /// <summary>Converts a <see cref="PersianDateTime"/> value to a <see cref="DateTime"/>.</summary>
    /// <param name="date">The Persian date and time to convert.</param>
    /// <returns>A <see cref="DateTime"/> representing the converted Gregorian date.</returns>
    DateTime ToDateTime(PersianDateTime date);

    /// <summary>Determines whether the specified Persian year is a leap year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <returns><c>true</c> if the year is a leap year; otherwise, <c>false</c>.</returns>
    bool IsLeapYear(int year);

    /// <summary>Returns the number of days in the specified Persian month and year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <param name="month">The Persian month (1–12).</param>
    /// <returns>The number of days in the specified month.</returns>
    int DaysInMonth(int year, int month);

    /// <summary>Returns the number of days in the specified Persian year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <returns>The number of days in the specified year.</returns>
    int DaysInYear(int year);

    /// <summary>Validates whether the specified Persian date is valid.</summary>
    /// <param name="year">The Persian year.</param>
    /// <param name="month">The Persian month.</param>
    /// <param name="day">The Persian day.</param>
    /// <returns><c>true</c> if the date is valid; otherwise, <c>false</c>.</returns>
    bool IsValidDate(int year, int month, int day);

    /// <summary>Formats the specified Persian date using the default format.</summary>
    /// <param name="date">The Persian date to format.</param>
    /// <returns>A string representation of the date.</returns>
    string Format(PersianDateTime date);

    /// <summary>Formats the specified Persian date using a custom format string.</summary>
    /// <param name="date">The Persian date to format.</param>
    /// <param name="format">The custom format string.</param>
    /// <returns>A string representation of the date.</returns>
    string Format(PersianDateTime date, string format);
}
