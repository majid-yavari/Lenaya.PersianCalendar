using System.Globalization;

namespace Lenaya.PersianCalendar;

internal static class PersianDateTimeParser
{
    internal static bool TryParse(string? value, out PersianDateTime result) =>
        TryParse(value, DateTimeKind.Unspecified, out result);

    internal static bool TryParse(string? value, DateTimeKind kind, out PersianDateTime result)
    {
        result = default;
        if (kind is not DateTimeKind.Unspecified and not DateTimeKind.Utc and not DateTimeKind.Local)
        {
            return false;
        }

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        if (TryParseShort(value, kind, out result))
        {
            return true;
        }

        if (TryParseCompact(value, kind, out result))
        {
            return true;
        }

        if (TryParseSanitized(value, kind, out result))
        {
            return true;
        }

        return false;
    }

    private static bool TryParseShort(string value, DateTimeKind kind, out PersianDateTime result)
    {
        result = default;
        if (value.Length < 10 || value[4] != '/' || value[7] != '/')
        {
            return false;
        }

        ReadOnlySpan<char> span = value;
        if (!int.TryParse(span[..4], out var year) ||
            !int.TryParse(span[5..7], out var month) ||
            !int.TryParse(span[8..10], out var day) ||
            !PersianDateValidation.IsValidDate(year, month, day))
        {
            return false;
        }

        if (value.Length == 10)
        {
            result = new PersianDateTime(year, month, day, kind);
            return true;
        }

        if (value.Length == 19 && value[10] == ' ' && span[13] == ':' && span[16] == ':')
        {
            if (!int.TryParse(span[11..13], out var hour) ||
                !int.TryParse(span[14..16], out var minute) ||
                !int.TryParse(span[17..19], out var second) ||
                !PersianDateValidation.IsValidTime(hour, minute, second))
            {
                return false;
            }

            result = new PersianDateTime(year, month, day, hour, minute, second, kind);
            return true;
        }

        return false;
    }

    private static bool TryParseCompact(string value, DateTimeKind kind, out PersianDateTime result)
    {
        result = default;
        if (value.Length != 14)
        {
            return false;
        }

        ReadOnlySpan<char> span = value;
        if (!int.TryParse(span[..4], out var year) ||
            !int.TryParse(span[4..6], out var month) ||
            !int.TryParse(span[6..8], out var day) ||
            !int.TryParse(span[8..10], out var hour) ||
            !int.TryParse(span[10..12], out var minute) ||
            !int.TryParse(span[12..14], out var second) ||
            !PersianDateValidation.IsValidDate(year, month, day) ||
            !PersianDateValidation.IsValidTime(hour, minute, second))
        {
            return false;
        }

        result = new PersianDateTime(year, month, day, hour, minute, second, kind);
        return true;
    }

    private static bool TryParseSanitized(string value, DateTimeKind kind, out PersianDateTime result)
    {
        result = default;
        Span<char> digits = stackalloc char[14];
        var count = 0;

        foreach (var character in value)
        {
            var digit = CharUnicodeInfo.GetDecimalDigitValue(character);
            if (digit < 0)
            {
                continue;
            }

            if (count >= digits.Length)
            {
                return false;
            }

            digits[count++] = (char)('0' + digit);
        }

        return count switch
        {
            8 => TryParseDateDigits(digits[..count], kind, out result),
            14 => TryParseCompact(new string(digits[..count]), kind, out result),
            _ => false
        };
    }

    private static bool TryParseDateDigits(ReadOnlySpan<char> span, DateTimeKind kind, out PersianDateTime result)
    {
        result = default;
        if (!int.TryParse(span[..4], out var year) ||
            !int.TryParse(span[4..6], out var month) ||
            !int.TryParse(span[6..8], out var day) ||
            !PersianDateValidation.IsValidDate(year, month, day))
        {
            return false;
        }

        result = new PersianDateTime(year, month, day, kind);
        return true;
    }
}
