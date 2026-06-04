namespace Lenaya.PersianCalendar;

/// <summary>Represents a time-of-day value (hour, minute, second, millisecond) without a date component.</summary>
public readonly struct PersianTime : IComparable<PersianTime>, IEquatable<PersianTime>, IFormattable
{
    /// <summary>Gets the hour component (0–23).</summary>
    public int Hour { get; }
    /// <summary>Gets the minute component (0–59).</summary>
    public int Minute { get; }
    /// <summary>Gets the second component (0–59).</summary>
    public int Second { get; }
    /// <summary>Gets the millisecond component (0–999).</summary>
    public int Millisecond { get; }

    /// <summary>Initializes a new instance of <see cref="PersianTime"/> with the specified components.</summary>
    /// <param name="hour">The hour (0–23).</param>
    /// <param name="minute">The minute (0–59).</param>
    /// <param name="second">The second (0–59, default 0).</param>
    /// <param name="millisecond">The millisecond (0–999, default 0).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any component is out of range.</exception>
    public PersianTime(int hour, int minute, int second = 0, int millisecond = 0)
    {
        PersianCalendarHelper.ValidateTime(hour, minute, second, millisecond);
        Hour = hour;
        Minute = minute;
        Second = second;
        Millisecond = millisecond;
    }

    /// <summary>Initializes a new instance of <see cref="PersianTime"/> from a <see cref="TimeSpan"/>.</summary>
    /// <param name="timeSpan">The time span to extract the time-of-day from (only the time portion is used).</param>
    public PersianTime(TimeSpan timeSpan)
        : this(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds)
    {
    }

    /// <summary>Converts this time to a <see cref="TimeSpan"/> representing the time-of-day.</summary>
    /// <returns>A <see cref="TimeSpan"/> equivalent to this time value.</returns>
    public TimeSpan ToTimeSpan() => new(0, Hour, Minute, Second, Millisecond);

    /// <summary>Explicitly converts a <see cref="PersianTime"/> to a <see cref="TimeSpan"/>.</summary>
    /// <param name="time">The time to convert.</param>
    /// <returns>A <see cref="TimeSpan"/> representing the same time-of-day.</returns>
    public static explicit operator TimeSpan(PersianTime time) => time.ToTimeSpan();

    /// <summary>Explicitly converts a <see cref="TimeSpan"/> to a <see cref="PersianTime"/>.</summary>
    /// <param name="timeSpan">The time span to convert.</param>
    /// <returns>A <see cref="PersianTime"/> representing the same time-of-day.</returns>
    public static explicit operator PersianTime(TimeSpan timeSpan) => new(timeSpan);

    /// <summary>Adds the specified number of hours to this time, wrapping around the 24-hour clock.</summary>
    /// <param name="hours">The number of hours to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianTime"/> that is the result of the operation.</returns>
    public PersianTime AddHours(int hours)
    {
        var totalMinutes = Hour * 60 + Minute + hours * 60;
        totalMinutes %= 1440;
        if (totalMinutes < 0)
        {
            totalMinutes += 1440;
        }

        var hour = totalMinutes / 60;
        var minute = totalMinutes % 60;
        return new PersianTime(hour, minute, Second, Millisecond);
    }

    /// <summary>Adds the specified number of minutes to this time, wrapping around the 24-hour clock.</summary>
    /// <param name="minutes">The number of minutes to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianTime"/> that is the result of the operation.</returns>
    public PersianTime AddMinutes(int minutes)
    {
        var totalMinutes = Hour * 60 + Minute + minutes;
        totalMinutes %= 1440;
        if (totalMinutes < 0)
        {
            totalMinutes += 1440;
        }

        var hour = totalMinutes / 60;
        var minute = totalMinutes % 60;
        return new PersianTime(hour, minute, Second, Millisecond);
    }

    /// <summary>Compares this time with another <see cref="PersianTime"/>.</summary>
    /// <param name="other">The time to compare with.</param>
    /// <returns>A value indicating the relative order: negative if earlier, zero if equal, positive if later.</returns>
    public int CompareTo(PersianTime other)
    {
        var comparison = Hour.CompareTo(other.Hour);
        if (comparison != 0) return comparison;
        comparison = Minute.CompareTo(other.Minute);
        if (comparison != 0) return comparison;
        comparison = Second.CompareTo(other.Second);
        if (comparison != 0) return comparison;
        return Millisecond.CompareTo(other.Millisecond);
    }

    /// <summary>Determines whether this time is equal to another <see cref="PersianTime"/>.</summary>
    /// <param name="other">The time to compare with.</param>
    /// <returns><c>true</c> if the values are equal; otherwise <c>false</c>.</returns>
    public bool Equals(PersianTime other) =>
        Hour == other.Hour && Minute == other.Minute && Second == other.Second && Millisecond == other.Millisecond;

    /// <summary>Determines whether this time is equal to another object.</summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="PersianTime"/> with the same value; otherwise <c>false</c>.</returns>
    public override bool Equals(object? obj) =>
        obj is PersianTime other && Equals(other);

    /// <summary>Returns the hash code for this time.</summary>
    /// <returns>A 32-bit signed hash code.</returns>
    public override int GetHashCode() =>
        HashCode.Combine(Hour, Minute, Second, Millisecond);

    /// <summary>Returns the string representation of this time in "HH:mm:ss" format.</summary>
    /// <returns>A formatted string like "14:05:30".</returns>
    public override string ToString() =>
        $"{Hour:D2}:{Minute:D2}:{Second:D2}";

    /// <summary>Returns the string representation of this time using the specified format.</summary>
    /// <param name="format">A format string (see <see cref="PersianTimeFormatter"/> for supported patterns).</param>
    /// <returns>A formatted string.</returns>
    public string ToString(string? format) =>
        PersianTimeFormatter.Format(this, format);

    /// <summary>Returns the string representation of this time with the specified format and format provider.</summary>
    /// <param name="format">A format string.</param>
    /// <param name="formatProvider">An <see cref="IFormatProvider"/> (ignored in this implementation).</param>
    /// <returns>A formatted string.</returns>
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) =>
        ToString(format);

    /// <summary>Adds a number of hours to a <see cref="PersianTime"/>, wrapping around the 24-hour clock.</summary>
    /// <param name="time">The original time.</param>
    /// <param name="hours">The number of hours to add.</param>
    /// <returns>A new <see cref="PersianTime"/> advanced by the specified hours.</returns>
    public static PersianTime operator +(PersianTime time, int hours) => time.AddHours(hours);

    /// <summary>Subtracts a number of hours from a <see cref="PersianTime"/>, wrapping around the 24-hour clock.</summary>
    /// <param name="time">The original time.</param>
    /// <param name="hours">The number of hours to subtract.</param>
    /// <returns>A new <see cref="PersianTime"/> moved back by the specified hours.</returns>
    public static PersianTime operator -(PersianTime time, int hours) => time.AddHours(-hours);

    /// <summary>Determines whether two <see cref="PersianTime"/> instances are equal.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns><c>true</c> if the times are equal; otherwise <c>false</c>.</returns>
    public static bool operator ==(PersianTime a, PersianTime b) => a.Equals(b);

    /// <summary>Determines whether two <see cref="PersianTime"/> instances are not equal.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns><c>true</c> if the times are not equal; otherwise <c>false</c>.</returns>
    public static bool operator !=(PersianTime a, PersianTime b) => !(a == b);

    /// <summary>Determines whether one <see cref="PersianTime"/> is earlier than another.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is earlier than <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator <(PersianTime a, PersianTime b) => a.CompareTo(b) < 0;

    /// <summary>Determines whether one <see cref="PersianTime"/> is later than another.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is later than <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator >(PersianTime a, PersianTime b) => a.CompareTo(b) > 0;

    /// <summary>Determines whether one <see cref="PersianTime"/> is earlier than or equal to another.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is earlier than or equal to <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator <=(PersianTime a, PersianTime b) => a.CompareTo(b) <= 0;

    /// <summary>Determines whether one <see cref="PersianTime"/> is later than or equal to another.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns><c>true</c> if <paramref name="a"/> is later than or equal to <paramref name="b"/>; otherwise <c>false</c>.</returns>
    public static bool operator >=(PersianTime a, PersianTime b) => a.CompareTo(b) >= 0;

    /// <summary>Adds the specified number of seconds to this time, wrapping around the 24-hour clock.</summary>
    /// <param name="seconds">The number of seconds to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianTime"/> that is the result of the operation.</returns>
    public PersianTime AddSeconds(int seconds)
    {
        var totalSeconds = Hour * 3600 + Minute * 60 + Second + seconds;
        totalSeconds %= 86400;
        if (totalSeconds < 0) totalSeconds += 86400;
        var hour = totalSeconds / 3600;
        var minute = (totalSeconds % 3600) / 60;
        var second = totalSeconds % 60;
        return new PersianTime(hour, minute, second, Millisecond);
    }

    /// <summary>Adds the specified number of milliseconds to this time, wrapping around the 24-hour clock.</summary>
    /// <param name="milliseconds">The number of milliseconds to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianTime"/> that is the result of the operation.</returns>
    public PersianTime AddMilliseconds(int milliseconds)
    {
        var totalMs = ((Hour * 3600L + Minute * 60L + Second) * 1000L + Millisecond + milliseconds) % 86400000L;
        if (totalMs < 0) totalMs += 86400000L;
        var hour = (int)(totalMs / 3600000);
        var minute = (int)((totalMs % 3600000) / 60000);
        var second = (int)((totalMs % 60000) / 1000);
        var ms = (int)(totalMs % 1000);
        return new PersianTime(hour, minute, second, ms);
    }

    /// <summary>Adds the specified number of ticks to this time, wrapping around the 24-hour clock (1 tick = 100 nanoseconds).</summary>
    /// <param name="ticks">The number of ticks to add (negative values subtract).</param>
    /// <returns>A new <see cref="PersianTime"/> that is the result of the operation.</returns>
    public PersianTime AddTicks(long ticks)
    {
        var totalTicks = ((Hour * 3600L + Minute * 60L + Second) * 1000L + Millisecond) * 10000L + ticks;
        totalTicks %= 86400L * 1000 * 10000;
        if (totalTicks < 0) totalTicks += 86400L * 1000 * 10000;
        var totalMs = totalTicks / 10000;
        var hour = (int)(totalMs / 3600000);
        var minute = (int)((totalMs % 3600000) / 60000);
        var second = (int)((totalMs % 60000) / 1000);
        var ms = (int)(totalMs % 1000);
        return new PersianTime(hour, minute, second, ms);
    }

    /// <summary>Determines whether this time is in the interval [start, end).</summary>
    /// <param name="start">The start of the interval (inclusive).</param>
    /// <param name="end">The end of the interval (exclusive).</param>
    /// <returns><c>true</c> if this time is greater than or equal to <paramref name="start"/> and less than <paramref name="end"/>; otherwise <c>false</c>.</returns>
    public bool IsBetween(PersianTime start, PersianTime end) =>
        start <= this && this < end;

    /// <summary>Determines whether this time is strictly earlier than another time.</summary>
    /// <param name="other">The time to compare with.</param>
    /// <returns><c>true</c> if this time is earlier; otherwise <c>false</c>.</returns>
    public bool IsBefore(PersianTime other) =>
        CompareTo(other) < 0;

    /// <summary>Determines whether this time is strictly later than another time.</summary>
    /// <param name="other">The time to compare with.</param>
    /// <returns><c>true</c> if this time is later; otherwise <c>false</c>.</returns>
    public bool IsAfter(PersianTime other) =>
        CompareTo(other) > 0;

    /// <summary>Deconstructs this time into its hour, minute, and second components.</summary>
    /// <param name="hour">The hour component.</param>
    /// <param name="minute">The minute component.</param>
    /// <param name="second">The second component.</param>
    public void Deconstruct(out int hour, out int minute, out int second)
    {
        hour = Hour;
        minute = Minute;
        second = Second;
    }

    /// <summary>Returns the earlier of two <see cref="PersianTime"/> values.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns>The earlier time.</returns>
    public static PersianTime Min(PersianTime a, PersianTime b) =>
        a < b ? a : b;

    /// <summary>Returns the later of two <see cref="PersianTime"/> values.</summary>
    /// <param name="a">The first time.</param>
    /// <param name="b">The second time.</param>
    /// <returns>The later time.</returns>
    public static PersianTime Max(PersianTime a, PersianTime b) =>
        a > b ? a : b;
}
