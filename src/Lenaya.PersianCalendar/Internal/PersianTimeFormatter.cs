namespace Lenaya.PersianCalendar;

internal static class PersianTimeFormatter
{
    internal static string Format(PersianTime time, string? format)
    {
        if (string.IsNullOrEmpty(format))
        {
            return time.ToString();
        }

        if (format.Length == 1)
        {
            return format[0] switch
            {
                'h' => $"{GetHour12(time.Hour)}",
                'H' => $"{time.Hour}",
                'm' => $"{time.Minute}",
                's' => $"{time.Second}",
                'f' => $"{time.Millisecond:D3}"[..1],
                'F' => FormatMillisecondsF(time.Millisecond, 1),
                't' => time.Hour < 12 ? "ق" : "ب",
                _ => throw new FormatException($"Unsupported format: '{format}'")
            };
        }

        var builder = new System.Text.StringBuilder();
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

            if ("hHmstfF".Contains(character))
            {
                var count = 1;
                while (index + 1 < format.Length && format[index + 1] == character)
                {
                    count++;
                    index++;
                }

                builder.Append(FormatToken(time, character, count));
                continue;
            }

            builder.Append(character);
        }

        return builder.ToString();
    }

    private static string FormatToken(PersianTime time, char specifier, int count)
    {
        return specifier switch
        {
            'H' => count == 1 ? $"{time.Hour}" : $"{time.Hour:D2}",
            'h' => count == 1 ? $"{GetHour12(time.Hour)}" : $"{GetHour12(time.Hour):D2}",
            'm' => count == 1 ? $"{time.Minute}" : $"{time.Minute:D2}",
            's' => count == 1 ? $"{time.Second}" : $"{time.Second:D2}",
            'f' => $"{time.Millisecond:D3}"[..Math.Min(count, 3)],
            'F' => FormatMillisecondsF(time.Millisecond, count),
            't' => count == 1 ? (time.Hour < 12 ? "ق" : "ب") : (time.Hour < 12 ? "ق.ظ" : "ب.ظ"),
            _ => throw new FormatException($"Unsupported format specifier: '{specifier}'")
        };
    }

    private static int GetHour12(int hour)
    {
        var normalized = hour % 12;
        return normalized == 0 ? 12 : normalized;
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

        return string.Empty;
    }
}
