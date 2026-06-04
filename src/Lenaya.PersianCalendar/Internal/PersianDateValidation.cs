namespace Lenaya.PersianCalendar;

internal static class PersianDateValidation
{
    internal static int DaysInMonth(int persianYear, int persianMonth)
    {
        if (persianMonth is < 1 or > 12)
        {
            ThrowOutOfRange(nameof(persianMonth));
        }

        return persianMonth switch
        {
            <= 6 => 31,
            <= 11 => 30,
            _ => PersianLeapYearCalculator.IsLeapYear(persianYear) ? 30 : 29
        };
    }

    internal static int DaysInYear(int persianYear) =>
        PersianLeapYearCalculator.IsLeapYear(persianYear) ? 366 : 365;

    internal static void ValidateDate(int year, int month, int day)
    {
        if (year is < 1 or > 9999)
        {
            ThrowOutOfRange(nameof(year));
        }

        if (month is < 1 or > 12)
        {
            ThrowOutOfRange(nameof(month));
        }

        if (day < 1 || day > DaysInMonth(year, month))
        {
            ThrowOutOfRange(nameof(day));
        }
    }

    internal static void ValidateTime(int hour, int minute, int second, int millisecond)
    {
        if (hour is < 0 or > 23)
        {
            ThrowOutOfRange(nameof(hour));
        }

        if (minute is < 0 or > 59)
        {
            ThrowOutOfRange(nameof(minute));
        }

        if (second is < 0 or > 59)
        {
            ThrowOutOfRange(nameof(second));
        }

        if (millisecond is < 0 or > 999)
        {
            ThrowOutOfRange(nameof(millisecond));
        }
    }

    internal static bool IsValidDate(int year, int month, int day)
    {
        if (year is < 1 or > 9999)
        {
            return false;
        }

        if (month is < 1 or > 12)
        {
            return false;
        }

        if (day < 1)
        {
            return false;
        }

        return day <= DaysInMonth(year, month);
    }

    internal static bool IsValidTime(int hour, int minute, int second, int millisecond = 0) =>
        hour is >= 0 and <= 23 &&
        minute is >= 0 and <= 59 &&
        second is >= 0 and <= 59 &&
        millisecond is >= 0 and <= 999;

    private static void ThrowOutOfRange(string paramName) =>
        throw new ArgumentOutOfRangeException(paramName);
}
