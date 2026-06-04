using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class PersianMonthTests
{
    [Fact]
    public void Constructor_ShouldCalculateCorrectProperties()
    {
        var month = new PersianMonth(1403, 1);
        Assert.Equal(1403, month.Year);
        Assert.Equal(1, month.Month);
        Assert.Equal(31, month.DayCount);
    }

    [Fact]
    public void GetDays_ShouldReturnAllDays()
    {
        var month = new PersianMonth(1403, 12);
        var days = month.GetDays().ToList();
        Assert.Equal(30, days.Count);
        Assert.Equal(new PersianDateTime(1403, 12, 1), days[0]);
        Assert.Equal(new PersianDateTime(1403, 12, 30), days[^1]);
    }

    [Fact]
    public void GetWeekGrid_ShouldReturnCorrectDimensions()
    {
        var month = new PersianMonth(1403, 1);
        var grid = month.GetWeekGrid();
        Assert.Equal(month.WeeksCount, grid.Length);
        foreach (var week in grid)
        {
            Assert.Equal(7, week.Length);
        }
    }

    [Fact]
    public void WeeksCount_ShouldBeReasonable()
    {
        var month = new PersianMonth(1403, 1);
        Assert.True(month.WeeksCount >= 4 && month.WeeksCount <= 6);
    }
}

