using System.Text;

namespace Lenaya.PersianCalendar;

internal static class PersianDateTimeFormatter
{
    private static readonly string[] StandardMonthNames =
    [
        "فروردین",
        "اردیبهشت",
        "خرداد",
        "تیر",
        "مرداد",
        "شهریور",
        "مهر",
        "آبان",
        "آذر",
        "دی",
        "بهمن",
        "اسفند"
    ];

    private static readonly string[] AstronomicalMonthNames =
    [
        "حمل",
        "ثور",
        "جوزا",
        "سرطان",
        "اسد",
        "سنبله",
        "میزان",
        "عقرب",
        "قوس",
        "جدی",
        "دلو",
        "حوت"
    ];

    private static readonly string[] AbbreviatedMonthNames =
    [
        "فرو",
        "ارد",
        "خرد",
        "تیر",
        "مرد",
        "شهر",
        "مهر",
        "آبا",
        "آذر",
        "دی",
        "بهم",
        "اسف"
    ];

    private static readonly string[] DayOfWeekNames =
    [
        "یکشنبه",
        "دوشنبه",
        "سه‌شنبه",
        "چهارشنبه",
        "پنجشنبه",
        "جمعه",
        "شنبه"
    ];

    private static readonly string[] AbbreviatedDayOfWeekNames =
    [
        "ی",
        "د",
        "س",
        "چ",
        "پ",
        "ج",
        "ش"
    ];

    internal static string GetStandardMonthName(int month)
    {
        ValidateMonth(month);
        return StandardMonthNames[month - 1];
    }

    internal static string GetAstronomicalMonthName(int month)
    {
        ValidateMonth(month);
        return AstronomicalMonthNames[month - 1];
    }

    internal static string Format(PersianDateTime date, string? format)
    {
        if (string.IsNullOrEmpty(format))
        {
            return date.ToShortDateString();
        }

        if (format.Length == 1)
        {
            return format[0] switch
            {
                'y' => $"{date.Year:D4}",
                'M' => $"{date.Month}",
                'd' => $"{date.Day}",
                'h' => $"{GetHour12(date.Hour)}",
                'H' => $"{date.Hour}",
                'm' => $"{date.Minute}",
                's' => $"{date.Second}",
                'f' => $"{date.Millisecond:D3}"[..1],
                'F' => FormatMillisecondsF(date.Millisecond, 1),
                't' => date.Hour < 12 ? "ق" : "ب",
                'g' => "ه.ش",
                'l' or 'L' => ToLongDateString(date, MonthNameStyle.Standard, PersianCalendarEra.SolarHijri),
                _ => throw new FormatException($"Unsupported format: '{format}'")
            };
        }

        var builder = new StringBuilder();
        for (var index = 0; index < format.Length; index++)
        {
            var character = format[index];

            if (character is '\'' or '"')
            {
                var delimiter = character;
                index++;
                while (index < format.Length && format[index] != delimiter)
                {
                    builder.Append(format[index]);
                    index++;
                }
                continue;
            }

            if (character == '\\')
            {
                index++;
                if (index < format.Length)
                {
                    builder.Append(format[index]);
                }
                continue;
            }

            if ("yMdHhmsfFgt".Contains(character))
            {
                var count = 1;
                while (index + 1 < format.Length && format[index + 1] == character)
                {
                    count++;
                    index++;
                }

                builder.Append(FormatToken(date, character, count));
                continue;
            }

            builder.Append(character);
        }

        return builder.ToString();
    }

    internal static string ToShortDateString(PersianDateTime date) =>
        $"{date.Year:D4}/{date.Month:D2}/{date.Day:D2}";

    internal static string ToShortDateTimeString(PersianDateTime date) =>
        $"{ToShortDateString(date)} {date.Hour:D2}:{date.Minute:D2}:{date.Second:D2}";

    internal static string To14DigitString(PersianDateTime date) =>
        $"{date.Year:D4}{date.Month:D2}{date.Day:D2}{date.Hour:D2}{date.Minute:D2}{date.Second:D2}";

    internal static string To8DigitString(PersianDateTime date) =>
        $"{date.Year:D4}{date.Month:D2}{date.Day:D2}";

    internal static string ToLongDateString(PersianDateTime date) =>
        ToLongDateString(date, MonthNameStyle.Standard, PersianCalendarEra.SolarHijri);

    internal static string ToLongDateString(PersianDateTime date, MonthNameStyle style, PersianCalendarEra era)
    {
        var monthNames = style switch
        {
            MonthNameStyle.Astronomical => AstronomicalMonthNames,
            _ => StandardMonthNames
        };

        var year = era switch
        {
            PersianCalendarEra.Shahanshahi => date.Year + 1180,
            _ => date.Year
        };

        var builder = new StringBuilder();
        builder.Append(date.DayOfWeek switch
        {
            DayOfWeek.Saturday => "شنبه",
            DayOfWeek.Sunday => "یکشنبه",
            DayOfWeek.Monday => "دوشنبه",
            DayOfWeek.Tuesday => "سه‌شنبه",
            DayOfWeek.Wednesday => "چهارشنبه",
            DayOfWeek.Thursday => "پنجشنبه",
            DayOfWeek.Friday => "جمعه",
            _ => ""
        });
        builder.Append(" ");
        builder.Append(date.Day);
        builder.Append(" ");
        builder.Append(monthNames[date.Month - 1]);
        builder.Append(" ");
        builder.Append(year);
        return builder.ToString();
    }

    internal static string ToLongDateTimeString(PersianDateTime date) =>
        $"{ToLongDateString(date)} ساعت {date.Hour:D2}:{date.Minute:D2}:{date.Second:D2}";

    internal static string ToLongDateTimeString(PersianDateTime date, MonthNameStyle style, PersianCalendarEra era) =>
        $"{ToLongDateString(date, style, era)} ساعت {date.Hour:D2}:{date.Minute:D2}:{date.Second:D2}";

    private static string FormatToken(PersianDateTime date, char specifier, int count)
    {
        return specifier switch
        {
            'y' => count <= 2 ? $"{date.Year % 100:D2}" : $"{date.Year:D4}",
            'M' => count switch
            {
                1 => $"{date.Month}",
                2 => $"{date.Month:D2}",
                3 => AbbreviatedMonthNames[date.Month - 1],
                _ => StandardMonthNames[date.Month - 1]
            },
            'd' => count switch
            {
                1 => $"{date.Day}",
                2 => $"{date.Day:D2}",
                3 => AbbreviatedDayOfWeekNames[(int)date.DayOfWeek],
                _ => DayOfWeekNames[(int)date.DayOfWeek]
            },
            'H' => count == 1 ? $"{date.Hour}" : $"{date.Hour:D2}",
            'h' => count == 1 ? $"{GetHour12(date.Hour)}" : $"{GetHour12(date.Hour):D2}",
            'm' => count == 1 ? $"{date.Minute}" : $"{date.Minute:D2}",
            's' => count == 1 ? $"{date.Second}" : $"{date.Second:D2}",
            'f' => $"{date.Millisecond:D3}"[..Math.Min(count, 3)],
            'F' => FormatMillisecondsF(date.Millisecond, count),
            't' => count == 1 ? (date.Hour < 12 ? "ق" : "ب") : (date.Hour < 12 ? "ق.ظ" : "ب.ظ"),
            'g' => "ه.ش",
            _ => throw new FormatException($"Unsupported format specifier: '{specifier}'")
        };
    }

    private static string FormatMillisecondsF(int milliseconds, int count)
    {
        var text = milliseconds.ToString("D3");
        count = Math.Min(count, 3);
        for (var index = count - 1; index >= 0; index--)
        {
            if (text[index] != '0')
            {
                return text[..(index + 1)];
            }
        }

        return "";
    }

    private static int GetHour12(int hour)
    {
        var normalized = hour % 12;
        return normalized == 0 ? 12 : normalized;
    }

    private static void ValidateMonth(int month)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month));
        }
    }
}
