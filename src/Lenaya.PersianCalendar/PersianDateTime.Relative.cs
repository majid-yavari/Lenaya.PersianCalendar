namespace Lenaya.PersianCalendar;

public readonly partial struct PersianDateTime
{

    /// <summary>Calculates the age in years from the birth date represented by this instance.</summary>
    /// <param name="from">The reference date to calculate the age from. Defaults to today.</param>
    /// <returns>The age in whole years. Returns 0 if the birth date is in the future.</returns>
    /// <example>
    /// <code>
    /// var birth = new PersianDateTime(1370, 1, 1);
    /// int age = birth.GetAge(); // Age relative to today
    /// </code>
    /// </example>
    public int GetAge(PersianDateTime? from = null)
    {
        var reference = from ?? Today;
        var age = reference.Year - Year;
        if (reference.Month < Month || (reference.Month == Month && reference.Day < Day))
        {
            age--;
        }

        return Math.Max(0, age);
    }

    /// <summary>Returns the age as a Persian text value.</summary>
    /// <param name="from">The reference date to calculate the age from. Defaults to today.</param>
    /// <param name="detailed">When true, includes years, months, and days instead of only whole years.</param>
    /// <returns>A Persian text representation of the age.</returns>
    public string GetAgeString(PersianDateTime? from = null, bool detailed = false)
    {
        var reference = from ?? Today;
        if (this > reference)
        {
            return detailed ? "0 روز" : "0 سال";
        }

        if (!detailed)
        {
            return $"{GetAge(reference)} سال";
        }

        var (years, months, days) = GetDatePartsDifference(this, reference);
        return FormatDateParts(years, months, days);
    }

    /// <summary>Returns a human-readable relative time string (e.g., "همین الان", "2 روز پیش", "3 ماه بعد") in Persian.</summary>
    /// <param name="from">The reference date-time. Defaults to now.</param>
    /// <param name="detailed">When true, date differences include years, months, and days.</param>
    /// <returns>A Persian relative time string describing the difference between this instance and the reference.</returns>
    /// <example>
    /// <code>
    /// var past = new PersianDateTime(1400, 1, 1);
    /// string relative = past.ToRelativeString(); // e.g., "2 سال پیش"
    /// </code>
    /// </example>
    public string ToRelativeString(PersianDateTime? from = null, bool detailed = false)
    {
        var reference = from ?? Now;
        var diffDays = (reference.ToDateTime().Date - ToDateTime().Date).Days;

        if (detailed && diffDays != 0)
        {
            var isFuture = diffDays < 0;
            var start = isFuture ? reference : this;
            var end = isFuture ? this : reference;
            var (years, months, days) = GetDatePartsDifference(start, end);
            var text = FormatDateParts(years, months, days);
            return $"{text} {(isFuture ? "بعد" : "پیش")}";
        }

        if (diffDays < 0)
        {
            var futureDays = -diffDays;
            if (futureDays < 7) return $"{futureDays} روز بعد";
            if (futureDays < 30) return $"{futureDays / 7} هفته بعد";
            var futureMonths = (reference.Year - Year) * 12 + reference.Month - Month;
            if (futureMonths < 0) futureMonths = -futureMonths;
            if (futureMonths < 12) return futureMonths == 0 ? "کمتر از یک ماه بعد" : $"{futureMonths} ماه بعد";
            var futureYears = GetAge(reference);
            if (futureYears == 0) return "کمتر از یک سال بعد";
            return $"{futureYears} سال بعد";
        }

        if (diffDays == 0)
        {
            var diffSeconds = (int)(reference.ToDateTime() - ToDateTime()).TotalSeconds;
            if (diffSeconds < 60) return "همین الان";
            var diffMinutes = diffSeconds / 60;
            if (diffMinutes < 60) return $"{diffMinutes} دقیقه پیش";
            var diffHours = diffMinutes / 60;
            if (diffHours <= 6) return $"{diffHours} ساعت پیش";
            return "امروز";
        }

        if (diffDays < 7) return $"{diffDays} روز پیش";
        if (diffDays < 30) return $"{diffDays / 7} هفته پیش";

        var monthsDiff = (reference.Year - Year) * 12 + reference.Month - Month;
        if (reference.Day < Day)
        {
            monthsDiff--;
        }

        if (monthsDiff < 12)
        {
            if (monthsDiff <= 0) return "کمتر از یک ماه پیش";
            return $"{monthsDiff} ماه پیش";
        }

        var yearsDiff = GetAge(reference);
        return yearsDiff == 0 ? "کمتر از یک سال پیش" : $"{yearsDiff} سال پیش";
    }


    private static (int Years, int Months, int Days) GetDatePartsDifference(PersianDateTime start, PersianDateTime end)
    {
        if (start > end)
        {
            (start, end) = (end, start);
        }

        var totalMonths = (end.Year - start.Year) * 12 + end.Month - start.Month;
        if (end.Day < start.Day)
        {
            totalMonths--;
        }

        if (totalMonths < 0)
        {
            totalMonths = 0;
        }

        var years = totalMonths / 12;
        var months = totalMonths % 12;
        var afterMonths = start.AddYears(years).AddMonths(months);
        var days = end - afterMonths;

        return (years, months, Math.Max(0, days));
    }

    private static string FormatDateParts(int years, int months, int days)
    {
        var parts = new List<string>(3);
        if (years > 0)
        {
            parts.Add($"{years} سال");
        }

        if (months > 0)
        {
            parts.Add($"{months} ماه");
        }

        if (days > 0 || parts.Count == 0)
        {
            parts.Add($"{days} روز");
        }

        return string.Join(" و ", parts);
    }
}
