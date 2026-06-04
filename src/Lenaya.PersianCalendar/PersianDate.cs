namespace Lenaya.PersianCalendar;

/// <summary>Represents a Persian (Solar Hijri) calendar date with year, month, and day components.</summary>
public readonly struct PersianDate : IComparable<PersianDate>, IEquatable<PersianDate>, IFormattable
{
    /// <summary>Gets the Persian year (1-based).</summary>
    public int Year { get; }
    /// <summary>Gets the Persian month (1 = Farvardin, 12 = Esfand).</summary>
    public int Month { get; }
    /// <summary>Gets the Persian day of the month (1-based).</summary>
    public int Day { get; }

    /// <summary>Initializes a new instance of <see cref="PersianDate"/> with the specified year, month, and day.</summary>
    /// <param name="year">The Persian year (1-based).</param>
    /// <param name="month">The Persian month (1 = Farvardin, 12 = Esfand).</param>
    /// <param name="day">The Persian day of the month (1-based).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the date is invalid.</exception>
    public PersianDate(int year, int month, int day)
    {
        PersianCalendarHelper.ValidateDate(year, month, day);
        Year = year;
        Month = month;
        Day = day;
    }

    /// <summary>Initializes a new instance of <see cref="PersianDate"/> from a <see cref="PersianDateTime"/>.</summary>
    /// <param name="date">The <see cref="PersianDateTime"/> value to extract the date from.</param>
    public PersianDate(PersianDateTime date)
    {
        Year = date.Year;
        Month = date.Month;
        Day = date.Day;
    }

    /// <summary>Initializes a new instance of <see cref="PersianDate"/> from a Gregorian <see cref="DateTime"/>.</summary>
    /// <param name="dateTime">The Gregorian <see cref="DateTime"/> value to convert.</param>
    public PersianDate(DateTime dateTime)
    {
        var (y, m, d) = PersianCalendarHelper.ToPersian(dateTime);
        Year = y;
        Month = m;
        Day = d;
    }

    /// <summary>Gets the <see cref="DayOfWeek"/> for this date in the Gregorian calendar.</summary>
    /// <value>The day of the week (Sunday = 0, Saturday = 6).</value>
    public DayOfWeek DayOfWeek
    {
        get
        {
            var dt = ToDateTime();
            return dt.DayOfWeek;
        }
    }

    /// <summary>Gets the day of the Persian year (1-based).</summary>
    /// <value>A value from 1 to 365 (or 366 in leap years).</value>
    public int DayOfYear
    {
        get
        {
            var days = 0;
            for (var m = 1; m < Month; m++)
            {
                days += PersianCalendarHelper.DaysInMonth(Year, m);
            }
            return days + Day;
        }
    }

    /// <summary>Gets the week number within the Persian year using Saturday as the default start of week.</summary>
    /// <value>The week number (1-based).</value>
    public int WeekNumber => GetWeekOfYear();

    /// <summary>Calculates the week of the year for this date.</summary>
    /// <param name="startOfWeek">The day considered the start of the week (default is Saturday).</param>
    /// <returns>The 1-based week number.</returns>
    public int GetWeekOfYear(DayOfWeek startOfWeek = DayOfWeek.Saturday)
    {
        var dayOfYear = DayOfYear;
        var firstDow = (int)new PersianDateTime(Year, 1, 1).DayOfWeek;
        var startDow = (int)startOfWeek;

        var offset = (startDow - firstDow + 7) % 7;

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

    /// <summary>Converts this date to a <see cref="PersianDateTime"/> with time set to midnight.</summary>
    /// <returns>A <see cref="PersianDateTime"/> representing the same date at 00:00:00.000.</returns>
    public PersianDateTime ToPersianDate() => new(Year, Month, Day);

    /// <summary>Converts this Persian date to the equivalent Gregorian <see cref="DateTime"/> at midnight.</summary>
    /// <returns>A <see cref="DateTime"/> representing the same date at 00:00:00.</returns>
    public DateTime ToDateTime() =>
        PersianCalendarHelper.ToGregorian(Year, Month, Day);

    /// <summary>Explicitly converts a <see cref="PersianDate"/> to a <see cref="PersianDateTime"/> at midnight.</summary>
    /// <param name="d">The date to convert.</param>
    /// <returns>A <see cref="PersianDateTime"/> with the same date and zero time.</returns>
    public static explicit operator PersianDateTime(PersianDate d) => d.ToPersianDate();

    /// <summary>Explicitly converts a <see cref="PersianDateTime"/> to a <see cref="PersianDate"/> by discarding the time component.</summary>
    /// <param name="d">The date-time value to convert.</param>
    /// <returns>A <see cref="PersianDate"/> with the same year, month, and day.</returns>
    public static explicit operator PersianDate(PersianDateTime d) => new(d);

    /// <summary>Explicitly converts a Gregorian <see cref="DateTime"/> to a <see cref="PersianDate"/>.</summary>
    /// <param name="dt">The Gregorian date-time to convert.</param>
    /// <returns>A <see cref="PersianDate"/> representing the same date.</returns>
    public static explicit operator PersianDate(DateTime dt) => new(dt);

    /// <summary>Adds the specified number of days to this date.</summary>
    /// <param name="days">The number of days to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianDate"/> that is the result of the operation.</returns>
    public PersianDate AddDays(int days)
    {
        if (days == 0)
        {
            return this;
        }
        var dt = ToDateTime().AddDays(days);
        return new PersianDate(dt);
    }

    /// <summary>Adds the specified number of months to this date.</summary>
    /// <param name="months">The number of months to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianDate"/> that is the result of the operation. If the resulting day exceeds the month's last day, it is clamped.</returns>
    public PersianDate AddMonths(int months)
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
        return new PersianDate(newYear, newMonth, newDay);
    }

    /// <summary>Adds the specified number of years to this date.</summary>
    /// <param name="years">The number of years to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianDate"/> that is the result of the operation. If the resulting day exceeds the month's last day (e.g., Feb 29 in a non-leap year), it is clamped.</returns>
    public PersianDate AddYears(int years)
    {
        if (years == 0)
        {
            return this;
        }
        var newYear = Year + years;
        var maxDay = PersianCalendarHelper.DaysInMonth(newYear, Month);
        var newDay = Math.Min(Day, maxDay);
        return new PersianDate(newYear, Month, newDay);
    }

    /// <summary>Compares this date with another <see cref="PersianDate"/>.</summary>
    /// <param name="other">The date to compare with.</param>
    /// <returns>A value indicating the relative order: negative if earlier, zero if equal, positive if later.</returns>
    public int CompareTo(PersianDate other)
    {
        var c = Year.CompareTo(other.Year);
        if (c != 0) return c;
        c = Month.CompareTo(other.Month);
        if (c != 0) return c;
        return Day.CompareTo(other.Day);
    }

    /// <summary>Determines whether this date is equal to another <see cref="PersianDate"/>.</summary>
    /// <param name="other">The date to compare with.</param>
    /// <returns><c>true</c> if the values are equal; otherwise <c>false</c>.</returns>
    public bool Equals(PersianDate other) =>
        Year == other.Year && Month == other.Month && Day == other.Day;

    /// <summary>Determines whether this date is equal to another object.</summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="PersianDate"/> with the same value; otherwise <c>false</c>.</returns>
    public override bool Equals(object? obj) =>
        obj is PersianDate other && Equals(other);

    /// <summary>Returns the hash code for this date.</summary>
    /// <returns>A 32-bit signed hash code.</returns>
    public override int GetHashCode() => HashCode.Combine(Year, Month, Day);

    /// <summary>Returns the string representation of this date in "yyyy/MM/dd" format.</summary>
    /// <returns>A formatted string like "1403/07/15".</returns>
    /// <example>new PersianDate(1403, 7, 15).ToString() // "1403/07/15"</example>
    public override string ToString() =>
        $"{Year:D4}/{Month:D2}/{Day:D2}";

    /// <summary>Returns the string representation of this date using the specified format.</summary>
    /// <param name="format">A format string (see <see cref="PersianDateTime.ToString(string)"/> for supported patterns).</param>
    /// <returns>A formatted string.</returns>
    public string ToString(string? format) =>
        ToPersianDate().ToString(format);

    /// <summary>Returns the string representation of this date with the specified format and format provider.</summary>
    /// <param name="format">A format string.</param>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> (ignored in this implementation).</param>
    /// <returns>A formatted string.</returns>
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) =>
        ToString(format);

    /// <summary>Determines whether two <see cref="PersianDate"/> instances are equal.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns><c>true</c> if the dates are equal; otherwise <c>false</c>.</returns>
    public static bool operator ==(PersianDate a, PersianDate b) => a.Equals(b);
    /// <summary>Determines whether two <see cref="PersianDate"/> instances are not equal.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns><c>true</c> if the dates are not equal; otherwise <c>false</c>.</returns>
    public static bool operator !=(PersianDate a, PersianDate b) => !(a == b);
    /// <summary>Determines whether one <see cref="PersianDate"/> is earlier than another.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is earlier than <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator <(PersianDate a, PersianDate b) => a.CompareTo(b) < 0;
    /// <summary>Determines whether one <see cref="PersianDate"/> is later than another.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is later than <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator >(PersianDate a, PersianDate b) => a.CompareTo(b) > 0;
    /// <summary>Determines whether one <see cref="PersianDate"/> is earlier than or equal to another.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is earlier than or equal to <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator <=(PersianDate a, PersianDate b) => a.CompareTo(b) <= 0;
    /// <summary>Determines whether one <see cref="PersianDate"/> is later than or equal to another.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is later than or equal to <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator >=(PersianDate a, PersianDate b) => a.CompareTo(b) >= 0;

    /// <summary>Adds a number of days to a <see cref="PersianDate"/>.</summary>
    /// <param name="date">The original date.</param>
    /// <param name="days">The number of days to add.</param>
    /// <returns>A new <see cref="PersianDate"/> advanced by the specified days.</returns>
    public static PersianDate operator +(PersianDate date, int days) => date.AddDays(days);
    /// <summary>Subtracts a number of days from a <see cref="PersianDate"/>.</summary>
    /// <param name="date">The original date.</param>
    /// <param name="days">The number of days to subtract.</param>
    /// <returns>A new <see cref="PersianDate"/> moved back by the specified days.</returns>
    public static PersianDate operator -(PersianDate date, int days) => date.AddDays(-days);
    /// <summary>Calculates the difference in days between two <see cref="PersianDate"/> values.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns>The number of full days between the two dates (may be negative).</returns>
    public static int operator -(PersianDate a, PersianDate b) =>
        (int)(a.ToDateTime() - b.ToDateTime()).TotalDays;

    /// <summary>Gets the current date in the Persian calendar.</summary>
    /// <value>A <see cref="PersianDate"/> representing today's date.</value>
    public static PersianDate Today => new(DateTime.Today);

    /// <summary>Determines whether the specified Persian year is a leap year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <returns><c>true</c> if the year is a leap year (has 366 days); otherwise <c>false</c>.</returns>
    public static bool IsLeapYear(int year) => PersianCalendarHelper.IsLeapYear(year);
    /// <summary>Returns the number of days in the specified Persian month.</summary>
    /// <param name="year">The Persian year.</param>
    /// <param name="month">The Persian month (1–12).</param>
    /// <returns>The number of days (29, 30, or 31).</returns>
    public static int DaysInMonth(int year, int month) => PersianCalendarHelper.DaysInMonth(year, month);
    /// <summary>Returns the number of days in the specified Persian year.</summary>
    /// <param name="year">The Persian year.</param>
    /// <returns>365 for a common year or 366 for a leap year.</returns>
    public static int DaysInYear(int year) => PersianCalendarHelper.DaysInYear(year);
    /// <summary>Determines whether the specified date components form a valid Persian date.</summary>
    /// <param name="year">The Persian year.</param>
    /// <param name="month">The Persian month.</param>
    /// <param name="day">The Persian day.</param>
    /// <returns><c>true</c> if the date is valid; otherwise <c>false</c>.</returns>
    public static bool IsValidDate(int year, int month, int day) => PersianDateTime.IsValidDate(year, month, day);

    /// <summary>Attempts to parse a string into a <see cref="PersianDate"/>.</summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">When successful, contains the parsed date; otherwise <c>default</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise <c>false</c>.</returns>
    public static bool TryParse(string s, out PersianDate result)
    {
        result = default;
        if (PersianDateTime.TryParse(s, out var pd))
        {
            result = new PersianDate(pd);
            return true;
        }
        return false;
    }

    /// <summary>Deconstructs this date into its year, month, and day components.</summary>
    /// <param name="year">The year component.</param>
    /// <param name="month">The month component.</param>
    /// <param name="day">The day component.</param>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = Year;
        month = Month;
        day = Day;
    }

    /// <summary>Returns the earlier of two <see cref="PersianDate"/> values.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns>The earlier date.</returns>
    public static PersianDate Min(PersianDate a, PersianDate b) =>
        a < b ? a : b;

    /// <summary>Returns the later of two <see cref="PersianDate"/> values.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns>The later date.</returns>
    public static PersianDate Max(PersianDate a, PersianDate b) =>
        a > b ? a : b;

    /// <summary>Determines whether this date falls on a weekend (Friday).</summary>
    /// <returns><c>true</c> if the day of the week is Friday; otherwise <c>false</c>.</returns>
    public bool IsWeekend() =>
        DayOfWeek is DayOfWeek.Friday;

    /// <summary>Gets the number of days in the month of this date.</summary>
    /// <returns>The day count for the current year and month.</returns>
    public int DaysInThisMonth() =>
        DaysInMonth(Year, Month);

    /// <summary>Gets the number of days in the year of this date.</summary>
    /// <returns>365 or 366 depending on whether the year is a leap year.</returns>
    public int DaysInThisYear() =>
        DaysInYear(Year);

    /// <summary>Gets the first day of the month for this date.</summary>
    /// <returns>A <see cref="PersianDate"/> representing the first day of the current month.</returns>
    public PersianDate StartOfMonth() => new(Year, Month, 1);

    /// <summary>Gets the last day of the month for this date.</summary>
    /// <returns>A <see cref="PersianDate"/> representing the last day of the current month.</returns>
    public PersianDate EndOfMonth() => new(Year, Month, DaysInMonth(Year, Month));

    /// <summary>Gets the first day of the year (Farvardin 1) for this date.</summary>
    /// <returns>A <see cref="PersianDate"/> representing the first day of the current year.</returns>
    public PersianDate StartOfYear() => new(Year, 1, 1);

    /// <summary>Gets the last day of the year (Esfand 29 or 30) for this date.</summary>
    /// <returns>A <see cref="PersianDate"/> representing the last day of the current year.</returns>
    public PersianDate EndOfYear() => new(Year, 12, DaysInMonth(Year, 12));
}
