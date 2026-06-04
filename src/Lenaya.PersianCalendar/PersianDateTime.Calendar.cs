namespace Lenaya.PersianCalendar;

public readonly partial struct PersianDateTime
{

    /// <summary>Gets the <see cref="DayOfWeek"/> represented by this instance.</summary>
    public DayOfWeek DayOfWeek => ToDateTime().DayOfWeek;

    /// <summary>Gets the week number of the year for this instance.</summary>
    /// <param name="startOfWeek">The <see cref="DayOfWeek"/> that starts the week. Defaults to Saturday.</param>
    /// <returns>The week number (1-53).</returns>
    public int GetWeekOfYear(DayOfWeek startOfWeek = DayOfWeek.Saturday)
    {
        var dayOfYear = DayOfYear;
        var firstDayOfWeek = (int)new PersianDateTime(Year, 1, 1).DayOfWeek;
        var start = (int)startOfWeek;

        var offset = (start - firstDayOfWeek + 7) % 7;

        if (offset == 0)
        {
            return ((dayOfYear - 1) / 7) + 1;
        }

        if (dayOfYear <= offset)
        {
            return 1;
        }

        return ((dayOfYear - offset - 1) / 7) + 2;
    }


    /// <summary>Gets the current date in the Persian calendar (local time). Time components are set to midnight.</summary>
    public static PersianDateTime Today => new(DateTime.Today);

    /// <summary>Gets the current date and time in the Persian calendar (local time).</summary>
    public static PersianDateTime Now => new(DateTime.Now);

    /// <summary>Gets the current date and time in the Persian calendar (UTC).</summary>
    public static PersianDateTime UtcNow => new(DateTime.UtcNow);

    /// <summary>Determines whether the specified Persian year is a leap year.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <returns><c>true</c> if the year is a leap year; otherwise, <c>false</c>.</returns>
    public static bool IsLeapYear(int year) =>
        PersianCalendarHelper.IsLeapYear(year);

    /// <summary>Returns the number of days in the specified Persian month and year.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <param name="month">The Persian month (1-12).</param>
    /// <returns>The number of days in the month (29, 30, or 31).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="month"/> is not between 1 and 12.</exception>
    public static int DaysInMonth(int year, int month) =>
        PersianCalendarHelper.DaysInMonth(year, month);

    /// <summary>Returns the number of days in the specified Persian year.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <returns>The number of days in the year (365 or 366).</returns>
    public static int DaysInYear(int year) =>
        PersianCalendarHelper.DaysInYear(year);

    /// <summary>Determines whether the specified Persian date is valid.</summary>
    /// <param name="year">The Persian year (1-9999).</param>
    /// <param name="month">The Persian month (1-12).</param>
    /// <param name="day">The Persian day (1-31).</param>
    /// <returns><c>true</c> if the date is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidDate(int year, int month, int day) =>
        PersianDateValidation.IsValidDate(year, month, day);


    /// <summary>Deconstructs this instance into its date components (year, month, day).</summary>
    /// <param name="year">When this method returns, contains the year component.</param>
    /// <param name="month">When this method returns, contains the month component.</param>
    /// <param name="day">When this method returns, contains the day component.</param>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = Year;
        month = Month;
        day = Day;
    }

    /// <summary>Deconstructs this instance into its date and time components (year, month, day, hour, minute, second).</summary>
    /// <param name="year">When this method returns, contains the year component.</param>
    /// <param name="month">When this method returns, contains the month component.</param>
    /// <param name="day">When this method returns, contains the day component.</param>
    /// <param name="hour">When this method returns, contains the hour component.</param>
    /// <param name="minute">When this method returns, contains the minute component.</param>
    /// <param name="second">When this method returns, contains the second component.</param>
    public void Deconstruct(out int year, out int month, out int day, out int hour, out int minute, out int second)
    {
        year = Year;
        month = Month;
        day = Day;
        hour = Hour;
        minute = Minute;
        second = Second;
    }

    /// <summary>Determines whether this instance falls on a weekend (Friday).</summary>
    /// <returns><c>true</c> if the day is Friday; otherwise, <c>false</c>.</returns>
    public bool IsWeekend() =>
        DayOfWeek is DayOfWeek.Friday;

    /// <summary>Gets the number of days in the month represented by this instance.</summary>
    /// <returns>The number of days (29, 30, or 31) in the current month.</returns>
    public int DaysInThisMonth() =>
        DaysInMonth(Year, Month);

    /// <summary>Gets the number of days in the year represented by this instance.</summary>
    /// <returns>The number of days (365 or 366) in the current year.</returns>
    public int DaysInThisYear() =>
        DaysInYear(Year);
}
