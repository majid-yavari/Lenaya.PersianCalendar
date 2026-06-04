namespace Lenaya.PersianCalendar;

/// <summary>
/// Specifies the type of a Persian holiday.
/// </summary>
public enum HolidayType
{
    /// <summary>
    /// A holiday that occurs on a fixed date in the Persian calendar.
    /// </summary>
    Fixed,
    /// <summary>
    /// A holiday determined by the lunar (Hijri) calendar.
    /// </summary>
    Lunar,
    /// <summary>
    /// A holiday based on solar astronomical events.
    /// </summary>
    Solar
}

/// <summary>
/// Represents a Persian holiday with its name, date, and type.
/// </summary>
public readonly record struct PersianHoliday(string Name, PersianDateTime Date, HolidayType Type);

public static class PersianHolidays
{
    private static readonly (int Month, int Day, string Name)[] _fixed =
    [
        (1, 1, "نوروز"),
        (1, 2, "نوروز"),
        (1, 3, "نوروز"),
        (1, 4, "نوروز"),
        (1, 12, "روز جمهوری اسلامی"),
        (1, 13, "روز طبیعت"),
        (3, 14, "رحلت امام خمینی"),
        (3, 15, "قیام ۱۵ خرداد"),
        (11, 22, "پیروزی انقلاب اسلامی")
    ];

    private static readonly (int HMonth, int HDay, string Name)[] _lunar =
    [
        (1, 9, "تاسوعای حسینی"),
        (1, 10, "عاشورای حسینی"),
        (2, 20, "اربعین حسینی"),
        (2, 28, "رحلت پیامبر اکرم و شهادت امام حسن مجتبی"),
        (3, 17, "ولادت پیامبر اکرم و امام صادق"),
        (7, 13, "ولادت امام علی"),
        (7, 27, "مبعث رسول اکرم"),
        (8, 15, "ولادت امام زمان"),
        (9, 19, "شب قدر"),
        (9, 21, "شهادت امام علی"),
        (10, 1, "عید سعید فطر"),
        (10, 2, "تعطیل عید فطر"),
        (12, 10, "عید سعید قربان"),
        (12, 18, "عید سعید غدیر")
    ];

    /// <summary>
    /// Returns a read-only list of all holidays for the specified Persian year.
    /// </summary>
    /// <param name="year">The Persian year to retrieve holidays for.</param>
    /// <returns>A read-only list of <see cref="PersianHoliday"/> instances for the given year.</returns>
    public static IReadOnlyList<PersianHoliday> GetHolidays(int year)
    {
        var list = new List<PersianHoliday>();

        foreach (var (m, d, name) in _fixed)
        {
            if (PersianDateTime.IsValidDate(year, m, d))
            {
                list.Add(new PersianHoliday(name, new PersianDateTime(year, m, d), HolidayType.Fixed));
            }
        }

        foreach (var (hm, hd, name) in _lunar)
        {
            var persianDate = LunarToPersian(year, hm, hd);
            if (persianDate.HasValue)
            {
                list.Add(new PersianHoliday(name, persianDate.Value, HolidayType.Lunar));
            }
        }

        return list.AsReadOnly();
    }

    /// <summary>
    /// Determines whether the specified date is a holiday.
    /// </summary>
    /// <param name="date">The Persian date to check.</param>
    /// <returns><c>true</c> if the date is a holiday; otherwise, <c>false</c>.</returns>
    public static bool IsHoliday(PersianDateTime date)
    {
        var holidays = GetHolidays(date.Year);
        return holidays.Any(h =>
            h.Date.Year == date.Year &&
            h.Date.Month == date.Month &&
            h.Date.Day == date.Day);
    }

    private static PersianDateTime? LunarToPersian(int persianYear, int hijriMonth, int hijriDay)
    {
        var persianStart = new PersianDateTime(persianYear, 1, 1);
        var gregStart = persianStart.ToDateTime();

        var hijriYear = HijriCalendarHelper.GregorianToHijri(gregStart).Year;

        for (var hy = hijriYear - 1; hy <= hijriYear + 1; hy++)
        {
            if (!HijriCalendarHelper.IsValidDate(hy, hijriMonth, hijriDay))
            {
                continue;
            }

            var greg = HijriCalendarHelper.HijriToGregorian(hy, hijriMonth, hijriDay);
            var persian = new PersianDateTime(greg);
            if (persian.Year == persianYear)
            {
                return persian;
            }
        }
        return null;
    }
}
