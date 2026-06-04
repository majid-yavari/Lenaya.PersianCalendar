namespace Lenaya.PersianCalendar;

internal static class JulianCalendarConverter
{
    private const long PersianEpochJulian = 1948320;

    internal static DateTime ToGregorian(int persianYear, int persianMonth, int persianDay)
    {
        PersianDateValidation.ValidateDate(persianYear, persianMonth, persianDay);
        var julian = PersianToJulian(persianYear, persianMonth, persianDay);
        return JulianToGregorian(julian);
    }

    internal static DateTime ToGregorian(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second, int millisecond = 0)
    {
        PersianDateValidation.ValidateDate(persianYear, persianMonth, persianDay);
        PersianDateValidation.ValidateTime(hour, minute, second, millisecond);
        var julian = PersianToJulian(persianYear, persianMonth, persianDay);
        var dateOnly = JulianToGregorian(julian);
        return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day, hour, minute, second, millisecond);
    }

    internal static (int Year, int Month, int Day) ToPersian(DateTime gregorian)
    {
        var julian = GregorianToJulian(gregorian.Year, gregorian.Month, gregorian.Day);
        return JulianToPersian(julian);
    }

    internal static long GregorianToJulian(int year, int month, int day)
    {
        var a = (14 - month) / 12;
        var y = year + 4800 - a;
        var m = month + 12 * a - 3;
        return day + (153L * m + 2) / 5 + 365L * y + y / 4 - y / 100 + y / 400 - 32045;
    }

    internal static DateTime JulianToGregorian(long julian)
    {
        var a = julian + 32044;
        var b = (4 * a + 3) / 146097;
        var c = a - (146097 * b) / 4;
        var d = (4 * c + 3) / 1461;
        var e = c - (1461 * d) / 4;
        var m = (int)((5 * e + 2) / 153);
        var day = (int)(e - (153L * m + 2) / 5 + 1);
        var month = m + 3 - 12 * (m / 10);
        var year = (int)(100 * b + d - 4800 + m / 10);
        return new DateTime(year, month, day);
    }

    private static long PersianToJulian(int year, int month, int day)
    {
        var epYear = year - 1;
        var days = epYear * 365L + PersianLeapYearCalculator.CountLeapYearsBefore(year);

        if (month <= 7)
        {
            days += (month - 1) * 31;
        }
        else
        {
            days += 6 * 31 + (month - 7) * 30;
        }

        days += day - 1;
        return PersianEpochJulian + days;
    }

    private static (int Year, int Month, int Day) JulianToPersian(long julian)
    {
        var daysSinceEpoch = julian - PersianEpochJulian;
        var approxYear = (int)(daysSinceEpoch / 365) + 1;

        var julianStart = PersianToJulian(approxYear, 1, 1);
        if (julian < julianStart)
        {
            approxYear--;
            julianStart = PersianToJulian(approxYear, 1, 1);
        }

        var dayOfYear = (int)(julian - julianStart);

        int month, day;
        if (dayOfYear < 186)
        {
            month = dayOfYear / 31 + 1;
            day = dayOfYear % 31 + 1;
        }
        else
        {
            var remaining = dayOfYear - 186;
            month = remaining / 30 + 7;
            day = remaining % 30 + 1;
        }

        return (approxYear, month, day);
    }
}
