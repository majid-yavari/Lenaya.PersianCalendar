namespace Lenaya.PersianCalendar;

/// <summary>
/// Represents a specific month in the Persian (Solar Hijri) calendar.
/// </summary>
public readonly struct PersianMonth : IComparable<PersianMonth>, IEquatable<PersianMonth>
{
    /// <summary>
    /// Gets the year of this month.
    /// </summary>
    public int Year { get; }
    /// <summary>
    /// Gets the month number (1-12).
    /// </summary>
    public int Month { get; }
    /// <summary>
    /// Gets the number of days in this month.
    /// </summary>
    public int DayCount { get; }
    /// <summary>
    /// Gets the day of the week for the first day of this month.
    /// </summary>
    public DayOfWeek FirstDayOfWeek { get; }
    /// <summary>
    /// Gets the number of weeks that span this month.
    /// </summary>
    public int WeeksCount { get; }

    /// <summary>Compares this month to another by year, then month.</summary>
    public int CompareTo(PersianMonth other)
    {
        var c = Year.CompareTo(other.Year);
        return c != 0 ? c : Month.CompareTo(other.Month);
    }

    /// <summary>Determines whether this month equals another.</summary>
    public bool Equals(PersianMonth other) =>
        Year == other.Year && Month == other.Month;

    /// <summary>Determines whether this month equals another object.</summary>
    public override bool Equals(object? obj) =>
        obj is PersianMonth other && Equals(other);

    /// <summary>Returns the hash code for this month.</summary>
    public override int GetHashCode() => HashCode.Combine(Year, Month);

    /// <summary>Returns true if two months are equal.</summary>
    public static bool operator ==(PersianMonth a, PersianMonth b) => a.Equals(b);
    /// <summary>Returns true if two months are not equal.</summary>
    public static bool operator !=(PersianMonth a, PersianMonth b) => !(a == b);
    /// <summary>Returns true if the left month is earlier.</summary>
    public static bool operator <(PersianMonth a, PersianMonth b) => a.CompareTo(b) < 0;
    /// <summary>Returns true if the left month is later.</summary>
    public static bool operator >(PersianMonth a, PersianMonth b) => a.CompareTo(b) > 0;
    /// <summary>Returns true if the left month is earlier or equal.</summary>
    public static bool operator <=(PersianMonth a, PersianMonth b) => a.CompareTo(b) <= 0;
    /// <summary>Returns true if the left month is later or equal.</summary>
    public static bool operator >=(PersianMonth a, PersianMonth b) => a.CompareTo(b) >= 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersianMonth"/> struct for the specified year and month.
    /// </summary>
    /// <param name="year">The Persian year.</param>
    /// <param name="month">The month number (1-12).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="month"/> is less than 1 or greater than 12.</exception>
    public PersianMonth(int year, int month)
    {
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year));
        }

        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month));
        }
        Year = year;
        Month = month;
        DayCount = PersianCalendarHelper.DaysInMonth(year, month);
        var firstOfMonth = new PersianDateTime(year, month, 1);
        FirstDayOfWeek = firstOfMonth.DayOfWeek;
        var totalCells = (int)FirstDayOfWeek + DayCount;
        WeeksCount = (totalCells + 6) / 7;
    }

    /// <summary>
    /// Returns a grid of day numbers for each week of the month, including padding days from adjacent months.
    /// </summary>
    /// <returns>A jagged array where each inner array represents a week containing 7 elements. Each element is a tuple with the day of month or null for padding and an is-padding flag.</returns>
    public (int? DayOfMonth, bool IsPadding)[][] GetWeekGrid()
    {
        var grid = new (int? DayOfMonth, bool IsPadding)[WeeksCount][];
        var day = 1;
        var startOffset = (int)FirstDayOfWeek;

        for (var w = 0; w < WeeksCount; w++)
        {
            grid[w] = new (int? DayOfMonth, bool IsPadding)[7];
            for (var d = 0; d < 7; d++)
            {
                if ((w == 0 && d < startOffset) || day > DayCount)
                {
                    grid[w][d] = (null, true);
                }
                else
                {
                    grid[w][d] = (day++, false);
                }
            }
        }
        return grid;
    }

    /// <summary>
    /// Returns a sequence of <see cref="PersianDateTime"/> values for each day in this month.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="PersianDateTime"/> instances for all days in this month.</returns>
    public IEnumerable<PersianDateTime> GetDays()
    {
        for (var d = 1; d <= DayCount; d++)
        {
            yield return new PersianDateTime(Year, Month, d);
        }
    }
}
