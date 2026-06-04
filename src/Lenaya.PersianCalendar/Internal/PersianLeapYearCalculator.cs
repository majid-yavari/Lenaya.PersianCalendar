namespace Lenaya.PersianCalendar;

internal static class PersianLeapYearCalculator
{
    private static readonly int[] LeapPositions = [1, 5, 9, 13, 17, 22, 26, 30];

    internal static bool IsLeapYear(int persianYear)
    {
        if (persianYear is < 1 or > 9999)
        {
            ThrowOutOfRange(nameof(persianYear));
        }

        var position = persianYear % 33;
        if (position == 0)
        {
            position = 33;
        }

        return Array.IndexOf(LeapPositions, position) >= 0;
    }

    internal static int CountLeapYearsBefore(int persianYear)
    {
        var yearBefore = persianYear - 1;
        var cycles = yearBefore / 33;
        var remaining = yearBefore % 33;

        var count = cycles * 8;
        for (var index = 0; index < LeapPositions.Length && LeapPositions[index] <= remaining; index++)
        {
            count++;
        }

        return count;
    }

    private static void ThrowOutOfRange(string paramName) =>
        throw new ArgumentOutOfRangeException(paramName);
}
