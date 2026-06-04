namespace Lenaya.PersianCalendar;

internal static class PersianCalendarHelper
{
    internal static bool IsLeapYear(int persianYear)
    {
        return PersianLeapYearCalculator.IsLeapYear(persianYear);
    }

    internal static int DaysInMonth(int persianYear, int persianMonth)
    {
        return PersianDateValidation.DaysInMonth(persianYear, persianMonth);
    }

    internal static int DaysInYear(int persianYear) =>
        PersianDateValidation.DaysInYear(persianYear);

    internal static void ValidateDate(int year, int month, int day)
    {
        PersianDateValidation.ValidateDate(year, month, day);
    }

    internal static void ValidateTime(int hour, int minute, int second, int millisecond)
    {
        PersianDateValidation.ValidateTime(hour, minute, second, millisecond);
    }

    internal static DateTime ToGregorian(int persianYear, int persianMonth, int persianDay)
    {
        return JulianCalendarConverter.ToGregorian(persianYear, persianMonth, persianDay);
    }

    internal static DateTime ToGregorian(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second, int millisecond = 0)
    {
        return JulianCalendarConverter.ToGregorian(persianYear, persianMonth, persianDay, hour, minute, second, millisecond);
    }

    internal static (int Year, int Month, int Day) ToPersian(DateTime gregorian)
    {
        return JulianCalendarConverter.ToPersian(gregorian);
    }

    internal static long GregorianToJulian(int year, int month, int day)
    {
        return JulianCalendarConverter.GregorianToJulian(year, month, day);
    }

    internal static DateTime JulianToGregorian(long julian)
    {
        return JulianCalendarConverter.JulianToGregorian(julian);
    }
}
