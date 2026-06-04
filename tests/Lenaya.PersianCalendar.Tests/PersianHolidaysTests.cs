using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class PersianHolidaysTests
{
    [Fact]
    public void GetHolidays_ShouldContainFixedHolidays()
    {
        var holidays = PersianHolidays.GetHolidays(1403);
        Assert.Contains(holidays, h => h.Name == "نوروز" && h.Date == new PersianDateTime(1403, 1, 1));
        Assert.Contains(holidays, h => h.Name == "پیروزی انقلاب اسلامی" && h.Date == new PersianDateTime(1403, 11, 22));
    }

    [Fact]
    public void GetHolidays_ShouldContainAshura()
    {
        var holidays = PersianHolidays.GetHolidays(1403);
        Assert.Contains(holidays, h => h.Name == "عاشورای حسینی");
    }

    [Fact]
    public void IsHoliday_FixedHoliday_ShouldReturnTrue()
    {
        Assert.True(PersianHolidays.IsHoliday(new PersianDateTime(1403, 1, 1)));
        Assert.True(PersianHolidays.IsHoliday(new PersianDateTime(1403, 1, 1, 14, 30, 0)));
        Assert.True(PersianHolidays.IsHoliday(new PersianDateTime(1403, 11, 22)));
    }

    [Fact]
    public void IsHoliday_NormalDay_ShouldReturnFalse()
    {
        Assert.False(PersianHolidays.IsHoliday(new PersianDateTime(1403, 2, 15)));
    }
}

