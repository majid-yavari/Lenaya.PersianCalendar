using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Lenaya.PersianCalendar;

BenchmarkRunner.Run<PersianCalendarBenchmarks>();

[MemoryDiagnoser]
public class PersianCalendarBenchmarks
{
    private static readonly PersianCalendar Bcl = new();
    private static readonly DateTime[] GregorianDates;
    private static readonly PersianDateTime[] PersianDates;

    static PersianCalendarBenchmarks()
    {
        var rng = new Random(42);
        var start = new DateTime(1900, 1, 1);
        GregorianDates = new DateTime[1000];
        PersianDates = new PersianDateTime[1000];

        for (var i = 0; i < 1000; i++)
        {
            GregorianDates[i] = start.AddDays(rng.Next(0, 50000));
            PersianDates[i] = new PersianDateTime(GregorianDates[i]);
        }
    }

    [Benchmark]
    public int Bcl_GregorianToPersian_Year()
    {
        var sum = 0;
        foreach (var dt in GregorianDates)
        {
            sum += Bcl.GetYear(dt);
        }
        return sum;
    }

    [Benchmark]
    public int Lenaya_GregorianToPersian_Year()
    {
        var sum = 0;
        foreach (var dt in GregorianDates)
        {
            sum += new PersianDateTime(dt).Year;
        }
        return sum;
    }

    [Benchmark]
    public int Bcl_PersianToGregorian_Ticks()
    {
        var sum = 0L;
        foreach (var pd in PersianDates)
        {
            sum += Bcl.ToDateTime(pd.Year, pd.Month, pd.Day, 0, 0, 0, 0).Ticks;
        }
        return (int)sum;
    }

    [Benchmark]
    public long Lenaya_PersianToGregorian_Ticks()
    {
        var sum = 0L;
        foreach (var pd in PersianDates)
        {
            sum += pd.ToDateTime().Ticks;
        }
        return sum;
    }

    [Benchmark]
    public bool Bcl_IsLeapYear()
    {
        var result = false;
        for (var y = 1; y <= 9378; y++)
        {
            result ^= Bcl.IsLeapYear(y);
        }
        return result;
    }

    [Benchmark]
    public bool Lenaya_IsLeapYear()
    {
        var result = false;
        for (var y = 1; y <= 9378; y++)
        {
            result ^= PersianDateTime.IsLeapYear(y);
        }
        return result;
    }

    [Benchmark]
    public int Bcl_DaysInMonth()
    {
        var sum = 0;
        for (var y = 1; y <= 5000; y++)
        {
            for (var m = 1; m <= 12; m++)
            {
                sum += Bcl.GetDaysInMonth(y, m);
            }
        }
        return sum;
    }

    [Benchmark]
    public int Lenaya_DaysInMonth()
    {
        var sum = 0;
        for (var y = 1; y <= 5000; y++)
        {
            for (var m = 1; m <= 12; m++)
            {
                sum += PersianDateTime.DaysInMonth(y, m);
            }
        }
        return sum;
    }

    [Benchmark]
    public int Bcl_ToDateTime_Full()
    {
        var sum = 0;
        for (var y = 1; y <= 2000; y++)
        {
            for (var m = 1; m <= 12; m++)
            {
                var d = Bcl.GetDaysInMonth(y, m);
                sum += Bcl.ToDateTime(y, m, d, 0, 0, 0, 0).Year;
            }
        }
        return sum;
    }

    [Benchmark]
    public int Lenaya_ToDateTime_Full()
    {
        var sum = 0;
        for (var y = 1; y <= 2000; y++)
        {
            for (var m = 1; m <= 12; m++)
            {
                var d = PersianDateTime.DaysInMonth(y, m);
                sum += new PersianDateTime(y, m, d).ToDateTime().Year;
            }
        }
        return sum;
    }
}
