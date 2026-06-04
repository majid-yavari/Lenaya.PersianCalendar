using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class PersianDateTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var d = new PersianDate(1403, 6, 15);
        Assert.Equal(1403, d.Year);
        Assert.Equal(6, d.Month);
        Assert.Equal(15, d.Day);
    }

    [Fact]
    public void Constructor_InvalidDate_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDate(1403, 6, 32));
    }

    [Fact]
    public void Constructor_FromPersianDate_ShouldStripTime()
    {
        var pd = new PersianDateTime(1403, 6, 15, 10, 30, 0);
        var d = new PersianDate(pd);
        Assert.Equal(1403, d.Year);
        Assert.Equal(6, d.Month);
        Assert.Equal(15, d.Day);
    }

    [Fact]
    public void Constructor_FromDateTime_ShouldWork()
    {
        var dt = new DateTime(2024, 9, 21);
        var d = new PersianDate(dt);
        Assert.Equal(new PersianDate(1403, 6, 31), d);
    }

    [Fact]
    public void ToString_Default_ShouldMatchPersianDate()
    {
        var d = new PersianDate(1403, 6, 15);
        Assert.Equal("1403/06/15", d.ToString());
    }

    [Fact]
    public void ToString_WithFormat_ShouldWork()
    {
        var d = new PersianDate(1403, 6, 15);
        Assert.Equal("شهریور", d.ToString("MMMM"));
        Assert.Equal("1403", d.ToString("yyyy"));
    }

    [Fact]
    public void ExplicitConversion_ToPersianDate_ShouldPreserve()
    {
        var d = new PersianDate(1403, 6, 15);
        PersianDateTime pd = (PersianDateTime)d;
        Assert.Equal(1403, pd.Year);
        Assert.Equal(6, pd.Month);
        Assert.Equal(15, pd.Day);
        Assert.Equal(0, pd.Hour);
    }

    [Fact]
    public void ExplicitConversion_FromPersianDate_ShouldStripTime()
    {
        var pd = new PersianDateTime(1403, 6, 15, 10, 30, 0);
        PersianDate d = (PersianDate)pd;
        Assert.Equal(1403, d.Year);
        Assert.Equal(6, d.Month);
        Assert.Equal(15, d.Day);
    }

    [Fact]
    public void AddDays_ShouldWork()
    {
        var d = new PersianDate(1403, 1, 1);
        Assert.Equal(new PersianDate(1403, 1, 11), d.AddDays(10));
        Assert.Equal(new PersianDate(1402, 12, 25), d.AddDays(-5));
    }

    [Fact]
    public void AddMonths_ShouldWork()
    {
        var d = new PersianDate(1403, 1, 1);
        Assert.Equal(new PersianDate(1403, 4, 1), d.AddMonths(3));
    }

    [Fact]
    public void AddYears_ShouldWork()
    {
        var d = new PersianDate(1403, 1, 1);
        Assert.Equal(new PersianDate(1404, 1, 1), d.AddYears(1));
    }

    [Fact]
    public void Comparison_ShouldWork()
    {
        var a = new PersianDate(1403, 1, 1);
        var b = new PersianDate(1403, 6, 15);
        Assert.True(a < b);
        Assert.True(b > a);
        Assert.Equal(a, a);
    }

    [Fact]
    public void Operator_PlusMinus_ShouldWork()
    {
        var d = new PersianDate(1403, 1, 1);
        Assert.Equal(new PersianDate(1403, 1, 8), d + 7);
        Assert.Equal(new PersianDate(1402, 12, 25), d - 5);
    }

    [Fact]
    public void Operator_Subtract_ShouldReturnDays()
    {
        var a = new PersianDate(1403, 12, 1);
        var b = new PersianDate(1403, 1, 1);
        Assert.Equal(336, a - b);
    }

    [Fact]
    public void Today_ShouldMatchPersianDate()
    {
        var expected = new PersianDate(PersianDateTime.Today);
        Assert.Equal(expected, PersianDate.Today);
    }

    [Fact]
    public void TryParse_ShouldWork()
    {
        Assert.True(PersianDate.TryParse("1403/06/15", out var d));
        Assert.Equal(new PersianDate(1403, 6, 15), d);
    }

    [Fact]
    public void StaticHelpers_ShouldMatchPersianDate()
    {
        Assert.Equal(PersianDateTime.IsLeapYear(1403), PersianDate.IsLeapYear(1403));
        Assert.Equal(PersianDateTime.DaysInMonth(1403, 6), PersianDate.DaysInMonth(1403, 6));
        Assert.Equal(PersianDateTime.DaysInYear(1403), PersianDate.DaysInYear(1403));
        Assert.False(PersianDate.IsValidDate(1403, 13, 1));
    }

    [Fact]
    public void DayOfYear_ShouldBeCorrect()
    {
        Assert.Equal(1, new PersianDate(1403, 1, 1).DayOfYear);
        Assert.Equal(32, new PersianDate(1403, 2, 1).DayOfYear);
    }
}

