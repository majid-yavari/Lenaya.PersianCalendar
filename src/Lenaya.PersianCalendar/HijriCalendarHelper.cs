namespace Lenaya.PersianCalendar;

internal static class HijriCalendarHelper
{
    private const long HijriEpochJulian = 1948440;

    private static readonly int[] _monthDays = [30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29];

    private static readonly int[] _leapYears = [2, 5, 7, 10, 13, 16, 18, 21, 24, 26, 29];

    private static readonly int[] _cumulativeDays =
    [
        0, 30, 59, 89, 118, 148, 177, 207, 236, 266, 295, 325
    ];

    internal static bool IsValidDate(int year, int month, int day) =>
        year >= 1 && month >= 1 && month <= 12 && day >= 1 && day <= DaysInMonth(year, month);

    internal static int DaysInMonth(int year, int month)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month));
        }

        if (month == 12 && IsLeapYear(year))
        {
            return 30;
        }
        return _monthDays[month - 1];
    }

    internal static int DaysInYear(int year) =>
        IsLeapYear(year) ? 355 : 354;

    internal static bool IsLeapYear(int year)
    {
        var pos = year % 30;
        if (pos == 0)
        {
            pos = 30;
        }
        return Array.IndexOf(_leapYears, pos) >= 0;
    }

    internal static int CountLeapYearsBefore(int year)
    {
        var prev = year - 1;
        var cycles = prev / 30;
        var remaining = prev % 30;
        var count = cycles * 11;
        for (var i = 0; i < _leapYears.Length && _leapYears[i] <= remaining; i++)
        {
            count++;
        }
        return count;
    }

    internal static long HijriToJulian(int year, int month, int day)
    {
        var days = (year - 1) * 354L + CountLeapYearsBefore(year);
        days += _cumulativeDays[month - 1];
        days += day - 1;
        return HijriEpochJulian + days;
    }

    internal static DateTime HijriToGregorian(int year, int month, int day)
    {
        var julian = HijriToJulian(year, month, day);
        return PersianCalendarHelper.JulianToGregorian(julian);
    }

    internal static (int Year, int Month, int Day) GregorianToHijri(DateTime gregorian)
    {
        var julian = PersianCalendarHelper.GregorianToJulian(gregorian.Year, gregorian.Month, gregorian.Day);
        return JulianToHijri(julian);
    }

    private static (int Year, int Month, int Day) JulianToHijri(long julian)
    {
        var daysSinceEpoch = julian - HijriEpochJulian;
        if (daysSinceEpoch < 0)
        {
            return (1, 1, 1);
        }

        var approxYear = (int)(daysSinceEpoch / 354) + 1;

        while (julian < HijriToJulian(approxYear, 1, 1))
        {
            approxYear--;
        }
        while (julian >= HijriToJulian(approxYear + 1, 1, 1))
        {
            approxYear++;
        }

        var dayOfYear = (int)(julian - HijriToJulian(approxYear, 1, 1));
        int month, day;
        for (month = 0; month < 12; month++)
        {
            var md = (month == 11 && IsLeapYear(approxYear)) ? 30 : _monthDays[month];
            if (dayOfYear < md)
            {
                break;
            }
            dayOfYear -= md;
        }
        day = dayOfYear + 1;
        month++;

        return (approxYear, month, day);
    }
}
