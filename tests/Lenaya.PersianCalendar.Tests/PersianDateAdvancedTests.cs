using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class PersianDateTimeAdvancedTests
{
    [Fact]
    public void TryParse_StandardFormat_ShouldReturnDate()
    {
        var result = PersianDateTime.TryParse("1403/01/01", out var date);
        Assert.True(result);
        Assert.Equal(new PersianDateTime(1403, 1, 1), date);
    }

    [Fact]
    public void TryParse_WithTimeFormat_ShouldIncludeTime()
    {
        var result = PersianDateTime.TryParse("1403/01/01 14:30:00", out var date);
        Assert.True(result);
        Assert.Equal(14, date.Hour);
        Assert.Equal(30, date.Minute);
    }

    [Fact]
    public void TryParse_14DigitFormat_ShouldReturnDate()
    {
        var result = PersianDateTime.TryParse("14030101143000", out var date);
        Assert.True(result);
        Assert.Equal(new PersianDateTime(1403, 1, 1, 14, 30, 0), date);
    }

    [Theory]
    [InlineData("1405/11/13 22:13:45")]
    [InlineData("1405-11-13T22:13:45")]
    [InlineData(" 1405 year 11 month 13 day 22:13:45 ")]
    [InlineData("۱۴۰۵/۱۱/۱۳ ۲۲:۱۳:۴۵")]
    public void TryParse_ShouldIgnoreNonDigitCharacters(string value)
    {
        var result = PersianDateTime.TryParse(value, DateTimeKind.Utc, out var date);

        Assert.True(result);
        Assert.Equal(new PersianDateTime(1405, 11, 13, 22, 13, 45, DateTimeKind.Utc), date);
        Assert.Equal(DateTimeKind.Utc, date.Kind);
    }

    [Fact]
    public void TryParse_DateOnly_WithKind_ShouldSetKind()
    {
        var result = PersianDateTime.TryParse("۱۴۰۵/۱۱/۱۳", DateTimeKind.Local, out var date);

        Assert.True(result);
        Assert.Equal(new PersianDateTime(1405, 11, 13, DateTimeKind.Local), date);
    }

    [Theory]
    [InlineData("1405/11/13 22:13:45")]
    [InlineData("1405-11-13T22:13:45")]
    [InlineData(" 1405 year 11 month 13 day 22:13:45 ")]
    [InlineData("۱۴۰۵/۱۱/۱۳ ۲۲:۱۳:۴۵")]
    public void Constructor_String_ShouldIgnoreNonDigitCharacters(string value)
    {
        var date = new PersianDateTime(value, DateTimeKind.Utc);

        Assert.Equal(1405, date.Year);
        Assert.Equal(11, date.Month);
        Assert.Equal(13, date.Day);
        Assert.Equal(22, date.Hour);
        Assert.Equal(13, date.Minute);
        Assert.Equal(45, date.Second);
        Assert.Equal(DateTimeKind.Utc, date.Kind);
    }

    [Fact]
    public void Constructor_String_DateOnly_ShouldWork()
    {
        var date = new PersianDateTime("1405/11/13", DateTimeKind.Local);

        Assert.Equal(new PersianDateTime(1405, 11, 13, DateTimeKind.Local), date);
        Assert.Equal(DateTimeKind.Local, date.Kind);
    }

    [Fact]
    public void Constructor_String_InvalidValue_ShouldThrowFormatException()
    {
        Assert.Throws<FormatException>(() => new PersianDateTime("1405/11"));
    }

    [Fact]
    public void TryParse_InvalidFormat_ShouldReturnFalse()
    {
        Assert.False(PersianDateTime.TryParse("", out _));
        Assert.False(PersianDateTime.TryParse(null, out _));
        Assert.False(PersianDateTime.TryParse("abc", out _));
        Assert.False(PersianDateTime.TryParse("140511", out _));
        Assert.False(PersianDateTime.TryParse("1403/13/01", out _));
        Assert.False(PersianDateTime.TryParse("1403/01/32", out _));
        Assert.False(PersianDateTime.TryParse("1405/11/31", out _));
        Assert.False(PersianDateTime.TryParse("1405/11/13 22:13:45:999", out _));
    }

    [Fact]
    public void TryParse_InvalidKind_ShouldReturnFalse()
    {
        var invalidKind = (DateTimeKind)42;

        Assert.False(PersianDateTime.TryParse("1405/11/13", invalidKind, out _));
    }

    [Fact]
    public void GetAge_BirthdayPassed_ShouldReturnCorrectAge()
    {
        var birth = new PersianDateTime(1366, 5, 22);
        var now = new PersianDateTime(1385, 6, 1);
        Assert.Equal(19, birth.GetAge(now));
    }

    [Fact]
    public void GetAge_BirthdayNotPassed_ShouldReturnOneLess()
    {
        var birth = new PersianDateTime(1366, 5, 22);
        var now = new PersianDateTime(1385, 4, 1);
        Assert.Equal(18, birth.GetAge(now));
    }

    [Fact]
    public void GetAge_ExactBirthday_ShouldReturnExact()
    {
        var birth = new PersianDateTime(1366, 5, 22);
        var now = new PersianDateTime(1385, 5, 22);
        Assert.Equal(19, birth.GetAge(now));
    }

    [Fact]
    public void GetAgeString_Detailed_ShouldIncludeYearsMonthsAndDays()
    {
        var birth = new PersianDateTime(1366, 5, 22);
        var now = new PersianDateTime(1404, 10, 4);

        Assert.Equal("38 سال و 4 ماه و 12 روز", birth.GetAgeString(now, detailed: true));
    }

    [Theory]
    [InlineData("1403/01/01", "1403/01/01", "همین الان")]
    [InlineData("1403/01/01", "1403/01/02", "1 روز پیش")]
    [InlineData("1403/01/01", "1403/01/08", "1 هفته پیش")]
    [InlineData("1403/01/01", "1403/02/01", "1 ماه پیش")]
    [InlineData("1403/01/01", "1404/01/01", "1 سال پیش")]
    [InlineData("1403/01/01", "1403/01/01 00:05:00", "5 دقیقه پیش")]
    public void ToRelativeString_ShouldReturnExpected(string fromStr, string toStr, string expected)
    {
        PersianDateTime.TryParse(fromStr, out var from);
        PersianDateTime.TryParse(toStr, out var to);
        var result = from.ToRelativeString(to);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToRelativeString_Detailed_ShouldIncludeYearsMonthsAndDays()
    {
        var from = new PersianDateTime(1366, 5, 22);
        var to = new PersianDateTime(1404, 10, 4);

        Assert.Equal("38 سال و 4 ماه و 12 روز پیش", from.ToRelativeString(to, detailed: true));
    }

    [Fact]
    public void Constructor_WithKind_ShouldSetKind()
    {
        var dateOnly = new PersianDateTime(1403, 1, 1, DateTimeKind.Utc);
        var dateTime = new PersianDateTime(1403, 1, 1, 14, 30, 0, DateTimeKind.Local);

        Assert.Equal(DateTimeKind.Utc, dateOnly.Kind);
        Assert.Equal(DateTimeKind.Local, dateTime.Kind);
    }

    [Fact]
    public void Constructor_DateTime_ShouldUseProvidedKindOnlyWhenUnspecified()
    {
        var unspecified = new PersianDateTime(new DateTime(2024, 3, 20, 10, 0, 0), DateTimeKind.Utc);
        var local = new PersianDateTime(new DateTime(2024, 3, 20, 10, 0, 0, DateTimeKind.Local), DateTimeKind.Utc);

        Assert.Equal(DateTimeKind.Utc, unspecified.Kind);
        Assert.Equal(DateTimeKind.Local, local.Kind);
    }

    [Fact]
    public void SqliteJulianDay_RoundTrip_ShouldPreserveValue()
    {
        var original = new PersianDateTime(1403, 1, 1, 14, 30, 45, DateTimeKind.Utc);
        var julianDay = original.ToSqliteJulianDay();
        var roundTrip = new PersianDateTime(julianDay, DateTimeKind.Utc);

        Assert.Equal(original, roundTrip);
        Assert.Equal(DateTimeKind.Utc, roundTrip.Kind);
    }

    [Fact]
    public void SqliteJulianDay_UnixEpoch_ShouldReturnKnownPersianDate()
    {
        var date = PersianDateTime.FromSqliteJulianDay(2440587.5, DateTimeKind.Utc);

        Assert.Equal(new PersianDateTime(1348, 10, 11, 0, 0, 0, DateTimeKind.Utc), date);
    }
}

