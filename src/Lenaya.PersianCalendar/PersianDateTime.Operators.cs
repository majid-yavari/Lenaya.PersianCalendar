namespace Lenaya.PersianCalendar;

public readonly partial struct PersianDateTime
{

    /// <summary>Adds the specified number of days to a <see cref="PersianDateTime"/>.</summary>
    /// <param name="date">The date to add days to.</param>
    /// <param name="days">The number of days to add.</param>
    /// <returns>A <see cref="PersianDateTime"/> that is the sum of the specified date and days.</returns>
    /// <example>
    /// <code>
    /// var result = new PersianDateTime(1400, 1, 1) + 10; // 1400/01/11
    /// </code>
    /// </example>
    public static PersianDateTime operator +(PersianDateTime date, int days) => date.AddDays(days);

    /// <summary>Subtracts the specified number of days from a <see cref="PersianDateTime"/>.</summary>
    /// <param name="date">The date to subtract days from.</param>
    /// <param name="days">The number of days to subtract.</param>
    /// <returns>A <see cref="PersianDateTime"/> that is the difference of the specified date and days.</returns>
    /// <example>
    /// <code>
    /// var result = new PersianDateTime(1400, 1, 11) - 10; // 1400/01/01
    /// </code>
    /// </example>
    public static PersianDateTime operator -(PersianDateTime date, int days) => date.AddDays(-days);

    /// <summary>Calculates the number of days between two <see cref="PersianDateTime"/> values.</summary>
    /// <param name="a">The first date.</param>
    /// <param name="b">The second date.</param>
    /// <returns>The number of whole days between the two dates.</returns>
    /// <example>
    /// <code>
    /// var days = new PersianDateTime(1400, 1, 11) - new PersianDateTime(1400, 1, 1); // 10
    /// </code>
    /// </example>
    public static int operator -(PersianDateTime a, PersianDateTime b) =>
        (int)Math.Round((a.ToDateTime() - b.ToDateTime()).TotalDays);

    /// <summary>Determines whether two <see cref="PersianDateTime"/> instances are equal.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns><c>true</c> if the values are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(PersianDateTime a, PersianDateTime b) =>
        a.Year == b.Year && a.Month == b.Month && a.Day == b.Day &&
        a.Hour == b.Hour && a.Minute == b.Minute && a.Second == b.Second &&
        a.Millisecond == b.Millisecond && a.Kind == b.Kind;

    /// <summary>Determines whether two <see cref="PersianDateTime"/> instances are not equal.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns><c>true</c> if the values are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(PersianDateTime a, PersianDateTime b) =>
        !(a == b);

    /// <summary>Determines whether one <see cref="PersianDateTime"/> is earlier than another.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is earlier than <paramref name="b"/>; otherwise, <c>false</c>.</returns>
    public static bool operator <(PersianDateTime a, PersianDateTime b) =>
        a.CompareTo(b) < 0;

    /// <summary>Determines whether one <see cref="PersianDateTime"/> is later than another.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is later than <paramref name="b"/>; otherwise, <c>false</c>.</returns>
    public static bool operator >(PersianDateTime a, PersianDateTime b) =>
        a.CompareTo(b) > 0;

    /// <summary>Determines whether one <see cref="PersianDateTime"/> is earlier than or equal to another.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is earlier than or equal to <paramref name="b"/>; otherwise, <c>false</c>.</returns>
    public static bool operator <=(PersianDateTime a, PersianDateTime b) =>
        a.CompareTo(b) <= 0;

    /// <summary>Determines whether one <see cref="PersianDateTime"/> is later than or equal to another.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is later than or equal to <paramref name="b"/>; otherwise, <c>false</c>.</returns>
    public static bool operator >=(PersianDateTime a, PersianDateTime b) =>
        a.CompareTo(b) >= 0;

    /// <summary>Explicitly converts a <see cref="PersianDateTime"/> to a <see cref="DateTime"/>.</summary>
    /// <param name="date">The <see cref="PersianDateTime"/> to convert.</param>
    /// <returns>A <see cref="DateTime"/> equivalent to the specified <see cref="PersianDateTime"/>.</returns>
    public static explicit operator DateTime(PersianDateTime date) => date.ToDateTime();

    /// <summary>Explicitly converts a <see cref="DateTime"/> to a <see cref="PersianDateTime"/>.</summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
    /// <returns>A <see cref="PersianDateTime"/> equivalent to the specified <see cref="DateTime"/>.</returns>
    public static explicit operator PersianDateTime(DateTime dateTime) => new(dateTime);

    /// <summary>Compares this instance to a specified <see cref="PersianDateTime"/> and indicates whether this instance is earlier, the same, or later.</summary>
    /// <param name="other">The <see cref="PersianDateTime"/> to compare with this instance.</param>
    /// <returns>A value less than zero if this instance is earlier than <paramref name="other"/>, zero if equal, or greater than zero if later.</returns>
    public int CompareTo(PersianDateTime other)
    {
        var comparison = Year.CompareTo(other.Year);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Month.CompareTo(other.Month);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Day.CompareTo(other.Day);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Hour.CompareTo(other.Hour);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Minute.CompareTo(other.Minute);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Second.CompareTo(other.Second);
        if (comparison != 0)
        {
            return comparison;
        }

        comparison = Millisecond.CompareTo(other.Millisecond);
        if (comparison != 0)
        {
            return comparison;
        }

        return Kind.CompareTo(other.Kind);
    }

    int IComparable.CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is PersianDateTime other)
        {
            return CompareTo(other);
        }

        throw new ArgumentException($"Cannot compare {nameof(PersianDateTime)} to {obj.GetType().Name}");
    }

    /// <summary>Determines whether this instance equals another <see cref="PersianDateTime"/> value.</summary>
    /// <param name="other">The <see cref="PersianDateTime"/> to compare with this instance.</param>
    /// <returns><c>true</c> if the values are equal; otherwise, <c>false</c>.</returns>
    public bool Equals(PersianDateTime other) =>
        Year == other.Year && Month == other.Month && Day == other.Day &&
        Hour == other.Hour && Minute == other.Minute && Second == other.Second &&
        Millisecond == other.Millisecond && Kind == other.Kind;

    /// <summary>Determines whether this instance equals the specified object.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="PersianDateTime"/> with the same value; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) =>
        obj is PersianDateTime other && Equals(other);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() =>
        HashCode.Combine(Year, Month, Day, Hour, Minute, Second, Millisecond, Kind);


    /// <summary>Returns the earlier of two <see cref="PersianDateTime"/> values.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns>The earlier of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static PersianDateTime Min(PersianDateTime a, PersianDateTime b) =>
        a < b ? a : b;

    /// <summary>Returns the later of two <see cref="PersianDateTime"/> values.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <returns>The later of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static PersianDateTime Max(PersianDateTime a, PersianDateTime b) =>
        a > b ? a : b;
}
