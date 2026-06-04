namespace Lenaya.PersianCalendar;

public readonly partial struct PersianDateTime
{

    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of days to this instance.</summary>
    /// <param name="days">The number of days to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified days.</returns>
    /// <example>
    /// <code>
    /// var date = new PersianDateTime(1400, 1, 1);
    /// var future = date.AddDays(10); // 1400/01/11
    /// </code>
    /// </example>
    public PersianDateTime AddDays(int days)
    {
        if (days == 0)
        {
            return this;
        }

        return new PersianDateTime(ToDateTime().AddDays(days));
    }

    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of months to this instance.</summary>
    /// <param name="months">The number of months to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified months.</returns>
    /// <remarks>The day is clamped to the last valid day of the resulting month if necessary.</remarks>
    /// <example>
    /// <code>
    /// var date = new PersianDateTime(1400, 1, 31);
    /// var result = date.AddMonths(1); // 1400/02/31 (clamped to 1400/02/31)
    /// </code>
    /// </example>
    public PersianDateTime AddMonths(int months)
    {
        if (months == 0)
        {
            return this;
        }

        var totalMonths = (Year - 1) * 12 + (Month - 1) + months;
        var newYear = totalMonths >= 0
            ? totalMonths / 12 + 1
            : (totalMonths - 11) / 12 + 1;
        var newMonth = ((totalMonths % 12) + 12) % 12 + 1;

        var maxDay = PersianCalendarHelper.DaysInMonth(newYear, newMonth);
        var newDay = Math.Min(Day, maxDay);
        return new PersianDateTime(newYear, newMonth, newDay, Hour, Minute, Second, Millisecond, Kind);
    }

    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of years to this instance.</summary>
    /// <param name="years">The number of years to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified years.</returns>
    /// <remarks>The day is clamped to the last valid day of the resulting month if necessary (e.g., adding a year to 1403/12/30 in a common year).</remarks>
    /// <example>
    /// <code>
    /// var date = new PersianDateTime(1400, 1, 1);
    /// var result = date.AddYears(1); // 1401/01/01
    /// </code>
    /// </example>
    public PersianDateTime AddYears(int years)
    {
        if (years == 0)
        {
            return this;
        }

        var newYear = Year + years;
        var maxDay = PersianCalendarHelper.DaysInMonth(newYear, Month);
        var newDay = Math.Min(Day, maxDay);
        return new PersianDateTime(newYear, Month, newDay, Hour, Minute, Second, Millisecond, Kind);
    }


    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of hours to this instance.</summary>
    /// <param name="value">The number of hours to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified hours.</returns>
    public PersianDateTime AddHours(double value) =>
        new(ToDateTime().AddHours(value));

    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of minutes to this instance.</summary>
    /// <param name="value">The number of minutes to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified minutes.</returns>
    public PersianDateTime AddMinutes(double value) =>
        new(ToDateTime().AddMinutes(value));

    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of seconds to this instance.</summary>
    /// <param name="value">The number of seconds to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified seconds.</returns>
    public PersianDateTime AddSeconds(double value) =>
        new(ToDateTime().AddSeconds(value));

    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of milliseconds to this instance.</summary>
    /// <param name="value">The number of milliseconds to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified milliseconds.</returns>
    public PersianDateTime AddMilliseconds(double value) =>
        new(ToDateTime().AddMilliseconds(value));

    /// <summary>Returns a new <see cref="PersianDateTime"/> that adds the specified number of ticks (100-nanosecond intervals) to this instance.</summary>
    /// <param name="value">The number of ticks to add. Can be negative to subtract.</param>
    /// <returns>A new <see cref="PersianDateTime"/> whose value is the sum of this instance and the specified ticks.</returns>
    public PersianDateTime AddTicks(long value) =>
        new(ToDateTime().AddTicks(value));

    /// <summary>Subtracts the specified <see cref="PersianDateTime"/> from this instance and returns the time difference.</summary>
    /// <param name="value">The <see cref="PersianDateTime"/> to subtract.</param>
    /// <returns>A <see cref="TimeSpan"/> representing the time difference.</returns>
    public TimeSpan Subtract(PersianDateTime value) =>
        ToDateTime() - value.ToDateTime();

    /// <summary>Returns a new <see cref="PersianDateTime"/> representing the start of the day (midnight) for this instance.</summary>
    /// <returns>A new <see cref="PersianDateTime"/> with the same date and time set to 00:00:00.000.</returns>
    public PersianDateTime StartOfDay() =>
        new(Year, Month, Day, 0, 0, 0, 0, Kind);

    /// <summary>Returns a new <see cref="PersianDateTime"/> representing the end of the day (23:59:59.999) for this instance.</summary>
    /// <returns>A new <see cref="PersianDateTime"/> with the same date and time set to 23:59:59.999.</returns>
    public PersianDateTime EndOfDay() =>
        new(Year, Month, Day, 23, 59, 59, 999, Kind);

    /// <summary>Returns a new <see cref="PersianDateTime"/> representing the first day of the month at midnight.</summary>
    /// <returns>A new <see cref="PersianDateTime"/> for the first day of the month with time set to 00:00:00.000.</returns>
    public PersianDateTime StartOfMonth() =>
        new(Year, Month, 1, 0, 0, 0, 0, Kind);

    /// <summary>Returns a new <see cref="PersianDateTime"/> representing the last day of the month at the end of the day.</summary>
    /// <returns>A new <see cref="PersianDateTime"/> for the last day of the month with time set to 23:59:59.999.</returns>
    public PersianDateTime EndOfMonth() =>
        new(Year, Month, DaysInMonth(Year, Month), 23, 59, 59, 999, Kind);

    /// <summary>Returns a new <see cref="PersianDateTime"/> representing the first day of the year at midnight.</summary>
    /// <returns>A new <see cref="PersianDateTime"/> for Farvardin 1st with time set to 00:00:00.000.</returns>
    public PersianDateTime StartOfYear() =>
        new(Year, 1, 1, 0, 0, 0, 0, Kind);

    /// <summary>Returns a new <see cref="PersianDateTime"/> representing the last day of the year at the end of the day.</summary>
    /// <returns>A new <see cref="PersianDateTime"/> for Esfand 29th/30th with time set to 23:59:59.999.</returns>
    public PersianDateTime EndOfYear() =>
        new(Year, 12, DaysInMonth(Year, 12), 23, 59, 59, 999, Kind);
}
