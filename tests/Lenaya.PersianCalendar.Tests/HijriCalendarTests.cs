using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class HijriCalendarTests
{
    [Fact]
    public void HijriToGregorian_KnownDate_ShouldMatch()
    {
        var greg = HijriCalendarHelper.HijriToGregorian(1446, 1, 1);
        Assert.Equal(new DateTime(2024, 7, 8), greg);
    }

    [Fact]
    public void GregorianToHijri_RoundTrip_ShouldPreserve()
    {
        var greg = new DateTime(2024, 3, 20);
        var (y, m, d) = HijriCalendarHelper.GregorianToHijri(greg);
        var back = HijriCalendarHelper.HijriToGregorian(y, m, d);
        Assert.Equal(greg, back);
    }

    [Fact]
    public void IsLeapYear_KnownLeapYears_ShouldReturnCorrect()
    {
        Assert.True(HijriCalendarHelper.IsLeapYear(2));
        Assert.True(HijriCalendarHelper.IsLeapYear(1447));
        Assert.False(HijriCalendarHelper.IsLeapYear(1));
        Assert.False(HijriCalendarHelper.IsLeapYear(1446));
    }
}

