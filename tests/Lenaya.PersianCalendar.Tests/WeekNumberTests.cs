using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class WeekNumberTests
{
    [Fact]
    public void GetWeekOfYear_Farvardin1_ShouldBeWeek1()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Equal(1, date.GetWeekOfYear());
        Assert.Equal(1, date.WeekNumber);
    }

    [Fact]
    public void GetWeekOfYear_SaturdayMidYear_ShouldStartNewWeek()
    {
        // 1403/01/04 is Saturday
        var saturday = new PersianDateTime(1403, 1, 4);
        var friday = new PersianDateTime(1403, 1, 3);
        Assert.Equal(saturday.WeekNumber, friday.WeekNumber + 1);
    }

    [Fact]
    public void GetWeekOfYear_EndOfYear_ShouldBeLastWeek()
    {
        // 1403 has 366 days (leap year)
        var lastDay = new PersianDateTime(1403, 12, 30);
        Assert.True(lastDay.WeekNumber >= 52);
    }

    [Fact]
    public void GetWeekOfYear_WithSundayStart_ShouldWork()
    {
        // Saturday (1403/01/04) is week boundary for Saturday-start but mid-week for Sunday-start
        var date = new PersianDateTime(1403, 1, 4);
        var weekSat = date.GetWeekOfYear(DayOfWeek.Saturday);
        var weekSun = date.GetWeekOfYear(DayOfWeek.Sunday);
        Assert.NotEqual(weekSat, weekSun);
    }

    [Fact]
    public void GetWeekOfYear_FirstSaturday_ShouldBeWeek2WhenNowruzNotSaturday()
    {
        // 1403/01/01 is Wednesday, so first Saturday (1403/01/04) should be week 2
        var firstSat = new PersianDateTime(1403, 1, 4);
        Assert.Equal(2, firstSat.GetWeekOfYear());
    }

    [Fact]
    public void DayOfYear_ShouldBeCorrect()
    {
        Assert.Equal(1, new PersianDateTime(1403, 1, 1).DayOfYear);
        Assert.Equal(32, new PersianDateTime(1403, 2, 1).DayOfYear);
        Assert.Equal(366, new PersianDateTime(1403, 12, 30).DayOfYear);
    }

    [Fact]
    public void DayOfYear_Farvardin31_ShouldBe31()
    {
        Assert.Equal(31, new PersianDateTime(1403, 1, 31).DayOfYear);
    }

    [Fact]
    public void GetWeekOfYear_1403AllDays_ShouldHaveValidWeeks()
    {
        for (var m = 1; m <= 12; m++)
        {
            var daysInMonth = PersianDateTime.DaysInMonth(1403, m);
            for (var d = 1; d <= daysInMonth; d++)
            {
                var date = new PersianDateTime(1403, m, d);
                var week = date.GetWeekOfYear();
                Assert.InRange(week, 1, 53);
            }
        }
    }
}

