using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class PersianTimeTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var t = new PersianTime(14, 30, 15, 500);
        Assert.Equal(14, t.Hour);
        Assert.Equal(30, t.Minute);
        Assert.Equal(15, t.Second);
        Assert.Equal(500, t.Millisecond);
    }

    [Fact]
    public void Constructor_InvalidTime_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianTime(24, 0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianTime(0, 60, 0));
    }

    [Fact]
    public void Constructor_FromTimeSpan_ShouldWork()
    {
        var ts = new TimeSpan(14, 30, 15);
        var t = new PersianTime(ts);
        Assert.Equal(14, t.Hour);
        Assert.Equal(30, t.Minute);
        Assert.Equal(15, t.Second);
    }

    [Fact]
    public void ToString_Default_ShouldReturnHHmmss()
    {
        var t = new PersianTime(14, 5, 3);
        Assert.Equal("14:05:03", t.ToString());
    }

    [Fact]
    public void ToString_WithFormat_ShouldWork()
    {
        var t = new PersianTime(14, 5, 3);
        Assert.Equal("14:05:03", t.ToString("HH:mm:ss"));
        Assert.Equal("02:05:03", t.ToString("hh:mm:ss"));
        Assert.Equal("2:05:03", t.ToString("h:mm:ss"));
        Assert.Equal("14", t.ToString("H"));
        Assert.Equal("5", t.ToString("m"));
    }

    [Fact]
    public void ToString_12HourFormat_ShouldWork()
    {
        var am = new PersianTime(9, 0, 0);
        var pm = new PersianTime(21, 0, 0);
        Assert.Contains("ق.ظ", am.ToString("hh:mm:ss tt"));
        Assert.Contains("ب.ظ", pm.ToString("hh:mm:ss tt"));
    }

    [Fact]
    public void AddHours_ShouldWrap()
    {
        var t = new PersianTime(22, 0, 0);
        Assert.Equal(new PersianTime(2, 0, 0), t.AddHours(4));
        Assert.Equal(new PersianTime(20, 0, 0), t.AddHours(-2));
    }

    [Fact]
    public void AddMinutes_ShouldWrap()
    {
        var t = new PersianTime(23, 50, 0);
        Assert.Equal(new PersianTime(0, 5, 0), t.AddMinutes(15));
        Assert.Equal(new PersianTime(23, 45, 0), t.AddMinutes(-5));
    }

    [Fact]
    public void Comparison_ShouldWork()
    {
        var a = new PersianTime(10, 0, 0);
        var b = new PersianTime(14, 30, 0);
        Assert.True(a < b);
        Assert.True(b > a);
        Assert.Equal(a, a);
    }

    [Fact]
    public void Operator_PlusMinus_ShouldWrap()
    {
        var t = new PersianTime(23, 0, 0);
        Assert.Equal(new PersianTime(2, 0, 0), t + 3);
        Assert.Equal(new PersianTime(21, 0, 0), t - 2);
    }

    [Fact]
    public void ExplicitConversion_ToTimeSpan_ShouldWork()
    {
        var t = new PersianTime(14, 30, 15);
        TimeSpan ts = (TimeSpan)t;
        Assert.Equal(14, ts.Hours);
        Assert.Equal(30, ts.Minutes);
    }

    [Fact]
    public void ExplicitConversion_FromTimeSpan_ShouldWork()
    {
        var ts = new TimeSpan(14, 30, 15);
        PersianTime t = (PersianTime)ts;
        Assert.Equal(14, t.Hour);
        Assert.Equal(30, t.Minute);
    }

    [Fact]
    public void GetHour12_Midnight_ShouldBe12()
    {
        var t = new PersianTime(0, 0, 0);
        Assert.Equal("12", t.ToString("h"));
        Assert.Equal("12", t.ToString("hh"));
        Assert.Equal("0", t.ToString("H"));
        Assert.Equal("00", t.ToString("HH"));
    }

    [Fact]
    public void MillisecondFormat_ShouldWork()
    {
        var t = new PersianTime(14, 30, 15, 120);
        Assert.Equal("120", t.ToString("fff"));
        Assert.Equal("12", t.ToString("ff"));
        Assert.Equal("1", t.ToString("f"));
    }

    [Fact]
    public void MillisecondFormat_F_Specifier_ShouldTrimTrailingZeros()
    {
        var t = new PersianTime(14, 30, 15, 120);
        Assert.Equal("12", t.ToString("FF"));
        Assert.Equal("1", t.ToString("F"));
        Assert.Equal("ss.FFF: 15.12", t.ToString("'ss.FFF: 'ss.FFF"));

        var zero = new PersianTime(14, 30, 15, 0);
        Assert.Equal("ss.FFF: 15.", zero.ToString("'ss.FFF: 'ss.FFF"));
        Assert.Equal("000", zero.ToString("fff"));
        Assert.Equal("", zero.ToString("F"));
        Assert.Equal("F: ", zero.ToString("'F: 'F"));
    }
}

