using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class PersianDateTimeTests
{
    // Known correct conversions (Gregorian -> Persian)
    // 1987-08-13 -> 1366-05-22 (My Birthday)
    // 2023-05-23 -> 1402-03-02 (Lena's Birthday)
    // 2024-03-20 -> 1403-01-01 (Norooz 1403)
    // 2024-03-19 -> 1402-12-29 (last day of 1402)
    // 2025-03-21 -> 1404-01-01 (Norooz 1404)
    // 1979-02-11 -> 1357-11-22 (Islamic Revolution)
    // 2021-09-21 -> 1400-06-30

    [Theory]
    [InlineData(1987, 8, 13, 1366, 5, 22)]
    [InlineData(2023, 5, 23, 1402, 3, 2)]
    [InlineData(2024, 3, 20, 1403, 1, 1)]
    [InlineData(2024, 3, 19, 1402, 12, 29)]
    [InlineData(2025, 3, 21, 1404, 1, 1)]
    [InlineData(1979, 2, 11, 1357, 11, 22)]
    [InlineData(2021, 9, 21, 1400, 6, 30)]
    [InlineData(2022, 3, 21, 1401, 1, 1)]
    [InlineData(2023, 3, 21, 1402, 1, 1)]
    [InlineData(2024, 9, 21, 1403, 6, 31)]
    [InlineData(2000, 1, 1, 1378, 10, 11)]
    [InlineData(2026, 6, 3, 1405, 3, 13)]
    public void GregorianToPersian_ShouldMatchExpected(
        int gy, int gm, int gd, int py, int pm, int pd)
    {
        var gregorian = new DateTime(gy, gm, gd);
        var persian = new PersianDateTime(gregorian);
        Assert.Equal(py, persian.Year);
        Assert.Equal(pm, persian.Month);
        Assert.Equal(pd, persian.Day);
    }

    [Theory]
    [InlineData(1366, 5, 22, 1987, 8, 13)]
    [InlineData(1402, 3, 2, 2023, 5, 23)]
    [InlineData(1403, 1, 1, 2024, 3, 20)]
    [InlineData(1402, 12, 29, 2024, 3, 19)]
    [InlineData(1404, 1, 1, 2025, 3, 21)]
    [InlineData(1357, 11, 22, 1979, 2, 11)]
    [InlineData(1400, 6, 30, 2021, 9, 21)]
    public void PersianToGregorian_ShouldMatchExpected(
        int py, int pm, int pd, int gy, int gm, int gd)
    {
        var persian = new PersianDateTime(py, pm, pd);
        var gregorian = persian.ToDateTime();
        Assert.Equal(new DateTime(gy, gm, gd), gregorian);
    }

    [Fact]
    public void RoundTrip_ShouldPreserveDate()
    {
        var original = new DateTime(2024, 6, 9);
        var persian = new PersianDateTime(original);
        var roundTrip = persian.ToDateTime();
        Assert.Equal(original, roundTrip);
    }

    [Theory]
    [InlineData(1403, 1, 1)]
    [InlineData(1378, 10, 11)]
    [InlineData(1357, 11, 22)]
    [InlineData(1403, 12, 30)]
    [InlineData(1404, 12, 29)]
    public void RoundTrip_PersianDate_ShouldPreserveDate(int y, int m, int d)
    {
        var persian = new PersianDateTime(y, m, d);
        var gregorian = persian.ToDateTime();
        var result = new PersianDateTime(gregorian);
        Assert.Equal(persian, result);
    }

    [Fact]
    public void Today_ShouldReturnCurrentDate()
    {
        var today = PersianDateTime.Today;
        var expected = new PersianDateTime(DateTime.Today);
        Assert.Equal(expected, today);
    }

    [Fact]
    public void Now_ShouldReturnCurrentDate()
    {
        var now = PersianDateTime.Now;
        var expected = new PersianDateTime(DateTime.Now);
        Assert.Equal(expected, now);
    }

    [Theory]
    [InlineData(1403, 4, 1)]
    [InlineData(1404, 7, 13)]
    [InlineData(1403, 12, 30)]
    public void ExplicitConversion_ToDateTime_ShouldBeEquivalent(int y, int m, int d)
    {
        var persian = new PersianDateTime(y, m, d);
        var dt = (DateTime)persian;
        Assert.Equal(persian.ToDateTime(), dt);
    }

    [Fact]
    public void ExplicitConversion_FromDateTime_ShouldBeEquivalent()
    {
        var dt = new DateTime(2024, 6, 9);
        var persian = (PersianDateTime)dt;
        Assert.Equal(new PersianDateTime(dt), persian);
    }

    [Theory]
    [InlineData(1403, 1, 31, true)]
    [InlineData(1403, 6, 31, true)]
    [InlineData(1403, 7, 30, true)]
    [InlineData(1403, 12, 29, true)]
    [InlineData(1403, 12, 30, true)]
    [InlineData(1403, 7, 31, false)]
    [InlineData(1404, 12, 30, false)]
    [InlineData(1404, 12, 31, false)]
    public void IsValidDate_ShouldReturnCorrectResult(int y, int m, int d, bool expected)
    {
        Assert.Equal(expected, PersianDateTime.IsValidDate(y, m, d));
    }

    [Theory]
    [InlineData(1403, true)]
    [InlineData(1404, false)]
    [InlineData(1400, false)]
    [InlineData(1399, true)]
    [InlineData(1395, true)]
    [InlineData(1398, false)]
    public void IsLeapYear_ShouldMatchExpected(int year, bool expected)
    {
        Assert.Equal(expected, PersianDateTime.IsLeapYear(year));
    }

    [Theory]
    [InlineData(1403, 1, 31)]
    [InlineData(1403, 6, 31)]
    [InlineData(1403, 7, 30)]
    [InlineData(1403, 11, 30)]
    [InlineData(1403, 12, 30)]
    [InlineData(1404, 12, 29)]
    [InlineData(1399, 12, 30)]
    public void DaysInMonth_ShouldReturnCorrectValue(int year, int month, int expected)
    {
        Assert.Equal(expected, PersianDateTime.DaysInMonth(year, month));
    }

    [Theory]
    [InlineData(1403, 366)]
    [InlineData(1404, 365)]
    [InlineData(1400, 365)]
    [InlineData(1399, 366)]
    public void DaysInYear_ShouldReturnCorrectValue(int year, int expected)
    {
        Assert.Equal(expected, PersianDateTime.DaysInYear(year));
    }

    [Fact]
    public void AddDays_Positive_ShouldAdvanceDate()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var result = date.AddDays(10);
        Assert.Equal(new PersianDateTime(1403, 1, 11), result);
    }

    [Fact]
    public void AddDays_Negative_ShouldGoBack()
    {
        var date = new PersianDateTime(1403, 1, 10);
        var result = date.AddDays(-5);
        Assert.Equal(new PersianDateTime(1403, 1, 5), result);
    }

    [Fact]
    public void AddDays_CrossYearBoundary_ShouldWork()
    {
        var date = new PersianDateTime(1403, 12, 25);
        var result = date.AddDays(10);
        Assert.Equal(new PersianDateTime(1404, 1, 5), result);
    }

    [Fact]
    public void AddDays_OperatorPlus_ShouldWork()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var result = date + 15;
        Assert.Equal(new PersianDateTime(1403, 1, 16), result);
    }

    [Fact]
    public void AddDays_OperatorMinus_ShouldWork()
    {
        var date = new PersianDateTime(1403, 1, 15);
        var result = date - 5;
        Assert.Equal(new PersianDateTime(1403, 1, 10), result);
    }

    [Fact]
    public void SubtractOperator_TwoDates_ShouldReturnDayDifference()
    {
        var a = new PersianDateTime(1404, 1, 1);
        var b = new PersianDateTime(1404, 1, 10);
        Assert.Equal(9, b - a);
        Assert.Equal(-9, a - b);
    }

    [Fact]
    public void AddMonths_Positive_ShouldAdvance()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var result = date.AddMonths(2);
        Assert.Equal(new PersianDateTime(1403, 3, 1), result);
    }

    [Fact]
    public void AddMonths_CrossYear_ShouldWork()
    {
        var date = new PersianDateTime(1403, 10, 1);
        var result = date.AddMonths(5);
        Assert.Equal(new PersianDateTime(1404, 3, 1), result);
    }

    [Fact]
    public void AddMonths_Negative_ShouldGoBack()
    {
        var date = new PersianDateTime(1403, 3, 1);
        var result = date.AddMonths(-2);
        Assert.Equal(new PersianDateTime(1403, 1, 1), result);
    }

    [Fact]
    public void AddMonths_Negative_CrossYear_ShouldWork()
    {
        var date = new PersianDateTime(1403, 3, 1);
        var result = date.AddMonths(-5);
        Assert.Equal(new PersianDateTime(1402, 10, 1), result);
    }

    [Fact]
    public void AddMonths_Negative_FromFarvardin_ShouldWrapToPreviousYear()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var result = date.AddMonths(-1);
        Assert.Equal(new PersianDateTime(1402, 12, 1), result);
    }

    [Fact]
    public void AddMonths_LargeNegative_ShouldJumpMultipleYears()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var result = date.AddMonths(-25);
        Assert.Equal(new PersianDateTime(1400, 12, 1), result);
    }

    [Fact]
    public void AddMonths_WithDayClamping_ShouldClampDay()
    {
        var date = new PersianDateTime(1403, 12, 29);
        var result = date.AddMonths(1);
        Assert.Equal(1404, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(29, result.Day);
    }

    [Fact]
    public void AddYears_Positive_ShouldWork()
    {
        var date = new PersianDateTime(1400, 1, 1);
        var result = date.AddYears(3);
        Assert.Equal(new PersianDateTime(1403, 1, 1), result);
    }

    [Fact]
    public void AddYears_LeapYearBoundary_ShouldClamp()
    {
        var date = new PersianDateTime(1403, 12, 30);
        var result = date.AddYears(1);
        Assert.Equal(new PersianDateTime(1404, 12, 29), result);
    }

    [Fact]
    public void EqualityOperator_SameDates_ShouldBeEqual()
    {
        var a = new PersianDateTime(1403, 6, 15);
        var b = new PersianDateTime(1403, 6, 15);
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void EqualityOperator_DifferentDates_ShouldNotBeEqual()
    {
        var a = new PersianDateTime(1403, 6, 15);
        var b = new PersianDateTime(1403, 6, 16);
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void EqualityOperator_DifferentKinds_ShouldNotBeEqual()
    {
        var utc = new PersianDateTime(1403, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        var local = new PersianDateTime(1403, 6, 15, 10, 0, 0, DateTimeKind.Local);

        Assert.False(utc == local);
        Assert.True(utc != local);
        Assert.False(utc.Equals(local));
    }

    [Fact]
    public void ComparisonOperators_ShouldWorkCorrectly()
    {
        var early = new PersianDateTime(1400, 1, 1);
        var late = new PersianDateTime(1400, 12, 29);

        Assert.True(early < late);
        Assert.True(late > early);
        Assert.True(early <= late);
        Assert.True(late >= early);
    }

    [Fact]
    public void CompareTo_SameDate_ShouldReturnZero()
    {
        var a = new PersianDateTime(1403, 6, 15);
        var b = new PersianDateTime(1403, 6, 15);
        Assert.Equal(0, a.CompareTo(b));
    }

    [Fact]
    public void CompareTo_EarlierDate_ShouldReturnNegative()
    {
        var a = new PersianDateTime(1403, 1, 1);
        var b = new PersianDateTime(1403, 6, 15);
        Assert.True(a.CompareTo(b) < 0);
    }

    [Fact]
    public void CompareTo_DifferentKinds_ShouldNotReturnZero()
    {
        var unspecified = new PersianDateTime(1403, 6, 15, 10, 0, 0, DateTimeKind.Unspecified);
        var utc = new PersianDateTime(1403, 6, 15, 10, 0, 0, DateTimeKind.Utc);

        Assert.NotEqual(0, unspecified.CompareTo(utc));
    }

    [Fact]
    public void Equals_SameDate_ShouldReturnTrue()
    {
        var a = new PersianDateTime(1403, 6, 15);
        var b = new PersianDateTime(1403, 6, 15);
        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
    }

    [Fact]
    public void Equals_Null_ShouldReturnFalse()
    {
        var a = new PersianDateTime(1403, 6, 15);
        Assert.False(a.Equals(null));
    }

    [Fact]
    public void GetHashCode_SameDates_ShouldMatch()
    {
        var a = new PersianDateTime(1403, 6, 15);
        var b = new PersianDateTime(1403, 6, 15);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentKinds_ShouldNotMatch()
    {
        var utc = new PersianDateTime(1403, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        var local = new PersianDateTime(1403, 6, 15, 10, 0, 0, DateTimeKind.Local);

        Assert.NotEqual(utc.GetHashCode(), local.GetHashCode());
    }

    [Fact]
    public void ToString_Default_ShouldBeFormatted()
    {
        var date = new PersianDateTime(1403, 1, 2,23,15,30);
        Assert.Equal("1403/01/02 23:15:30", date.ToString());
    }

    [Fact]
    public void ToString_WithFormat_ShouldReturnFormatted()
    {
        var date = new PersianDateTime(1403, 1, 2);
        Assert.Equal("1403", date.ToString("yyyy"));
        Assert.Equal("03", date.ToString("yy"));
        Assert.Equal("01", date.ToString("MM"));
        Assert.Equal("02", date.ToString("dd"));
    }

    [Fact]
    public void ToLongDateString_ShouldContainPersianWords()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var longStr = date.ToLongDateString();
        Assert.Contains("فروردین", longStr);
        Assert.Contains("چهارشنبه", longStr);
        Assert.Contains("1403", longStr);
    }

    [Fact]
    public void InvalidDate_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 1, 32));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 13, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 0, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 7, 31));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1404, 12, 30));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(0, 1, 1));
    }

    [Fact]
    public void Constructor_FromDateTime_Midnight_ShouldWork()
    {
        var dt = new DateTime(2024, 3, 20, 0, 0, 0);
        var persian = new PersianDateTime(dt);
        Assert.Equal(new PersianDateTime(1403, 1, 1), persian);
    }

    [Fact]
    public void DayOfWeek_ForKnownDate_ShouldMatch()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var dt = date.ToDateTime();
        Assert.Equal(dt.DayOfWeek, date.DayOfWeek);
    }

    [Fact]
    public void IComparable_CompareToObject_ShouldWork()
    {
        IComparable a = new PersianDateTime(1403, 1, 1);
        IComparable b = new PersianDateTime(1403, 1, 2);
        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.Equal(0, a.CompareTo(a));
    }

    [Fact]
    public void IComparable_CompareToNull_ShouldReturnPositive()
    {
        IComparable a = new PersianDateTime(1403, 1, 1);
        Assert.True(a.CompareTo(null) > 0);
    }

    [Fact]
    public void IComparable_CompareToInvalidType_ShouldThrow()
    {
        IComparable a = new PersianDateTime(1403, 1, 1);
        Assert.Throws<ArgumentException>(() => a.CompareTo("not a date"));
    }

    [Fact]
    public void AddDays_Zero_ShouldReturnSame()
    {
        var date = new PersianDateTime(1403, 6, 15);
        var result = date.AddDays(0);
        Assert.Equal(date, result);
    }

    [Fact]
    public void AddMonths_Zero_ShouldReturnSame()
    {
        var date = new PersianDateTime(1403, 6, 15);
        var result = date.AddMonths(0);
        Assert.Equal(date, result);
    }

    [Fact]
    public void AddYears_Zero_ShouldReturnSame()
    {
        var date = new PersianDateTime(1403, 6, 15);
        var result = date.AddYears(0);
        Assert.Equal(date, result);
    }

    [Fact]
    public void LargeDateRange_ShouldConvertCorrectly()
    {
        var dt = new DateTime(1900, 1, 1);
        var persian = new PersianDateTime(dt);
        var back = persian.ToDateTime();
        Assert.Equal(dt, back);
    }

    [Fact]
    public void PersianMonthBoundaries_ShouldConvertCorrectly()
    {
        for (var month = 1; month <= 12; month++)
        {
            Assert.True(PersianDateTime.IsValidDate(1403, month, 1));
            var lastDay = PersianDateTime.DaysInMonth(1403, month);
            Assert.True(PersianDateTime.IsValidDate(1403, month, lastDay));
        }
    }

    [Theory]
    [InlineData(1366, 5, 22, 14, 30, 0, 1987, 8, 13, 14, 30, 0)]
    [InlineData(1402, 3, 2, 9, 15, 45, 2023, 5, 23, 9, 15, 45)]
    public void PersianToGregorian_WithTime_ShouldMatchExpected(
        int py, int pm, int pd, int ph, int pmin, int ps,
        int gy, int gm, int gd, int gh, int gmin, int gs)
    {
        var persian = new PersianDateTime(py, pm, pd, ph, pmin, ps);
        var expected = new DateTime(gy, gm, gd, gh, gmin, gs);
        Assert.Equal(expected, persian.ToDateTime());
    }

    [Theory]
    [InlineData(1987, 8, 13, 20, 45, 10, 1366, 5, 22, 20, 45, 10)]
    [InlineData(2023, 5, 23, 6, 30, 0, 1402, 3, 2, 6, 30, 0)]
    public void GregorianToPersian_WithTime_ShouldMatchExpected(
        int gy, int gm, int gd, int gh, int gmin, int gs,
        int py, int pm, int pd, int ph, int pmin, int ps)
    {
        var gregorian = new DateTime(gy, gm, gd, gh, gmin, gs);
        var persian = new PersianDateTime(gregorian);
        Assert.Equal(py, persian.Year);
        Assert.Equal(pm, persian.Month);
        Assert.Equal(pd, persian.Day);
        Assert.Equal(ph, persian.Hour);
        Assert.Equal(pmin, persian.Minute);
        Assert.Equal(ps, persian.Second);
    }

    [Fact]
    public void Constructor_WithTime_ShouldSetTimeProperly()
    {
        var date = new PersianDateTime(1403, 6, 15, 10, 30, 45, 500);
        Assert.Equal(10, date.Hour);
        Assert.Equal(30, date.Minute);
        Assert.Equal(45, date.Second);
        Assert.Equal(500, date.Millisecond);
    }

    [Fact]
    public void TimeOfDay_ShouldMatchDateTime()
    {
        var persian = new PersianDateTime(1403, 6, 15, 14, 35, 20);
        Assert.Equal(new TimeSpan(14, 35, 20), persian.TimeOfDay);
    }

    [Fact]
    public void ToShortDateTimeString_ShouldIncludeTime()
    {
        var date = new PersianDateTime(1403, 1, 2, 8, 5, 3);
        Assert.Equal("1403/01/02 08:05:03", date.ToShortDateTimeString());
    }

    [Fact]
    public void ToLongDateTimeString_ShouldIncludeTime()
    {
        var date = new PersianDateTime(1403, 1, 1, 14, 30, 0);
        var result = date.ToLongDateTimeString();
        Assert.Contains("فروردین", result);
        Assert.Contains("ساعت", result);
        Assert.Contains("1403", result);
        Assert.Contains("14:30:00", result);
    }

    [Fact]
    public void Constructor_FromDateTime_ShouldPreserveKind()
    {
        var utc = new DateTime(2024, 3, 20, 10, 30, 0, DateTimeKind.Utc);
        var local = new DateTime(2024, 3, 20, 10, 30, 0, DateTimeKind.Local);

        var utcDate = new PersianDateTime(utc);
        var localDate = new PersianDateTime(local);

        Assert.Equal(DateTimeKind.Utc, utcDate.Kind);
        Assert.Equal(DateTimeKind.Local, localDate.Kind);
    }

    [Fact]
    public void ToUtcDateTime_And_ToLocalDateTime_ShouldConvertKinds()
    {
        var utcDate = new PersianDateTime(new DateTime(2024, 3, 20, 10, 30, 0, DateTimeKind.Utc));
        var localDate = new PersianDateTime(new DateTime(2024, 3, 20, 10, 30, 0, DateTimeKind.Local));

        Assert.Equal(DateTimeKind.Utc, utcDate.ToUtcDateTime().Kind);
        Assert.Equal(DateTimeKind.Local, utcDate.ToLocalDateTime().Kind);
        Assert.Equal(DateTimeKind.Utc, localDate.ToUtcDateTime().Kind);
        Assert.Equal(DateTimeKind.Local, localDate.ToLocalDateTime().Kind);
    }

    [Fact]
    public void RoundTrip_WithTime_ShouldPreserveTime()
    {
        var original = new DateTime(2024, 6, 9, 15, 45, 30, 200);
        var persian = new PersianDateTime(original);
        var roundTrip = persian.ToDateTime();
        Assert.Equal(original, roundTrip);
    }

    [Fact]
    public void AddDays_WithTime_ShouldPreserveTime()
    {
        var date = new PersianDateTime(1403, 1, 1, 10, 30, 0);
        var result = date.AddDays(5);
        Assert.Equal(new PersianDateTime(1403, 1, 6, 10, 30, 0), result);
    }

    [Fact]
    public void AddMonths_WithTime_ShouldPreserveTime()
    {
        var date = new PersianDateTime(1403, 1, 1, 14, 15, 30);
        var result = date.AddMonths(2);
        Assert.Equal(new PersianDateTime(1403, 3, 1, 14, 15, 30), result);
    }

    [Fact]
    public void AddYears_WithTime_ShouldPreserveTime()
    {
        var date = new PersianDateTime(1400, 1, 1, 9, 0, 0);
        var result = date.AddYears(3);
        Assert.Equal(new PersianDateTime(1403, 1, 1, 9, 0, 0), result);
    }

    [Fact]
    public void Equality_WithDifferentTime_ShouldNotBeEqual()
    {
        var a = new PersianDateTime(1403, 6, 15, 10, 0, 0);
        var b = new PersianDateTime(1403, 6, 15, 14, 0, 0);
        Assert.NotEqual(a, b);
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void CompareTo_WithTime_ShouldConsiderTime()
    {
        var early = new PersianDateTime(1403, 6, 15, 8, 0, 0);
        var late = new PersianDateTime(1403, 6, 15, 20, 0, 0);
        Assert.True(early < late);
        Assert.True(late > early);
    }

    [Fact]
    public void InvalidTime_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 1, 1, 24, 0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 1, 1, 0, 60, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 1, 1, 0, 0, 60));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PersianDateTime(1403, 1, 1, 0, 0, 0, 1000));
    }

    [Fact]
    public void ToString_Format_HHmmss_ShouldReturnTimeParts()
    {
        var date = new PersianDateTime(1403, 1, 2, 8, 5, 3);
        Assert.Equal("08", date.ToString("HH"));
        Assert.Equal("05", date.ToString("mm"));
        Assert.Equal("03", date.ToString("ss"));
    }

    [Fact]
    public void ToString_CustomFormat_CombinedPattern_ShouldWork()
    {
        var date = new PersianDateTime(1403, 1, 2, 8, 5, 3);
        Assert.Equal("1403/01/02", date.ToString("yyyy/MM/dd"));
        Assert.Equal("1403-01-02 08:05:03", date.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Fact]
    public void ToString_CustomFormat_dddd_ShouldReturnDayName()
    {
        // 1403/01/19 = 2024-04-07 which is a Sunday
        var date = new PersianDateTime(1403, 1, 19);
        Assert.Equal("یکشنبه", date.ToString("dddd"));
        Assert.Equal("ی", date.ToString("ddd"));
    }

    [Fact]
    public void ToString_CustomFormat_MMMM_ShouldReturnMonthName()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Equal("فروردین", date.ToString("MMMM"));
        Assert.Equal("فرو", date.ToString("MMM"));
    }

    [Fact]
    public void ToString_CustomFormat_gg_ShouldReturnEra()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Equal("ه.ش", date.ToString("gg"));
        Assert.Equal("ه.ش", date.ToString("g"));
    }

    [Fact]
    public void ToString_CustomFormat_tt_ShouldReturnAmPm()
    {
        var am = new PersianDateTime(1403, 1, 1, 9, 0, 0);
        var pm = new PersianDateTime(1403, 1, 1, 21, 0, 0);
        Assert.Equal("ق.ظ", am.ToString("tt"));
        Assert.Equal("ق", am.ToString("t"));
        Assert.Equal("ب.ظ", pm.ToString("tt"));
        Assert.Equal("ب", pm.ToString("t"));
    }

    [Fact]
    public void ToString_CustomFormat_hh_ShouldReturn12Hour()
    {
        var midnight = new PersianDateTime(1403, 1, 1, 0, 0, 0);
        var noon = new PersianDateTime(1403, 1, 1, 12, 0, 0);
        var afternoon = new PersianDateTime(1403, 1, 1, 21, 5, 0);
        Assert.Equal("12", midnight.ToString("hh"));
        Assert.Equal("12", midnight.ToString("h"));
        Assert.Equal("12", noon.ToString("hh"));
        Assert.Equal("9", afternoon.ToString("h"));
        Assert.Equal("09", afternoon.ToString("hh"));
    }

    [Fact]
    public void ToString_CustomFormat_H_ShouldReturn24HourNoLeadingZero()
    {
        var date = new PersianDateTime(1403, 1, 1, 8, 5, 3);
        Assert.Equal("8", date.ToString("H"));
        Assert.Equal("08", date.ToString("HH"));
    }

    [Fact]
    public void ToString_CustomFormat_ms_WithoutLeadingZero_ShouldWork()
    {
        var date = new PersianDateTime(1403, 1, 2, 8, 5, 3);
        Assert.Equal("5", date.ToString("m"));
        Assert.Equal("3", date.ToString("s"));
    }

    [Fact]
    public void ToString_CustomFormat_f_ShouldReturnMilliseconds()
    {
        var date = new PersianDateTime(1403, 1, 1, 0, 0, 0, 123);
        Assert.Equal("1", date.ToString("f"));
        Assert.Equal("12", date.ToString("ff"));
        Assert.Equal("123", date.ToString("fff"));
    }

    [Fact]
    public void ToString_CustomFormat_F_ShouldTrimTrailingZeros()
    {
        var date = new PersianDateTime(1403, 1, 1, 0, 0, 0, 100);
        Assert.Equal("1", date.ToString("F"));
        Assert.Equal("1", date.ToString("FF"));
        Assert.Equal("1", date.ToString("FFF"));
        var date2 = new PersianDateTime(1403, 1, 1, 0, 0, 0, 120);
        Assert.Equal("12", date2.ToString("FF"));
    }

    [Fact]
    public void ToString_CustomFormat_LiteralQuotes_ShouldPreserveText()
    {
        var date = new PersianDateTime(1403, 1, 2);
        Assert.Equal("Year: 1403", date.ToString("'Year: 'yyyy"));
        Assert.Equal("اسد 1403", date.ToString("\"اسد \"yyyy"));
    }

    [Fact]
    public void ToString_CustomFormat_EscapeChar_ShouldWork()
    {
        var date = new PersianDateTime(1403, 1, 2);
        Assert.Equal("Year 1403", date.ToString("\\Year yyyy"));
    }

    [Fact]
    public void ToString_CustomFormat_UnknownSpecifier_ShouldPassThrough()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Equal("zzz", date.ToString("zzz"));
    }

    [Fact]
    public void Now_ShouldIncludeCurrentTime()
    {
        var now = PersianDateTime.Now;
        var expected = new PersianDateTime(DateTime.Now);
        Assert.Equal(expected.Hour, now.Hour);
        Assert.Equal(expected.Minute, now.Minute);
    }

    [Fact]
    public void To14DigitString_ShouldReturnExpectedFormat()
    {
        var date1 = new PersianDateTime(1403, 1, 2, 8, 5, 3);
        Assert.Equal("14030102080503", date1.To14DigitString());

        var date2 = new PersianDateTime(1366, 5, 22, 20, 45, 10);
        Assert.Equal("13660522204510", date2.To14DigitString());

        var date3 = new PersianDateTime(1402, 3, 2, 6, 30, 0);
        Assert.Equal("14020302063000", date3.To14DigitString());
    }

    [Fact]
    public void To8DigitString_ShouldReturnExpectedFormat()
    {
        var date1 = new PersianDateTime(1403, 1, 2, 8, 5, 3);
        Assert.Equal("14030102", date1.To8DigitString());

        var date2 = new PersianDateTime(1366, 5, 22, 20, 45, 10);
        Assert.Equal("13660522", date2.To8DigitString());

        var date3 = new PersianDateTime(1402, 3, 2);
        Assert.Equal("14020302", date3.To8DigitString());
    }

    [Fact]
    public void YearEra_SolarHijri_ShouldReturnSameYear()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Equal(1403, date.YearEra(PersianCalendarEra.SolarHijri));
    }

    [Fact]
    public void YearEra_Shahanshahi_ShouldReturnYearPlus1180()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Equal(2583, date.YearEra(PersianCalendarEra.Shahanshahi));
    }

    [Theory]
    [InlineData(1403, PersianCalendarEra.SolarHijri, 1403)]
    [InlineData(1403, PersianCalendarEra.Shahanshahi, 2583)]
    [InlineData(1366, PersianCalendarEra.Shahanshahi, 2546)]
    [InlineData(1402, PersianCalendarEra.Shahanshahi, 2582)]
    [InlineData(1, PersianCalendarEra.Shahanshahi, 1181)]
    public void GetYearEra_ShouldReturnCorrectValue(int year, PersianCalendarEra era, int expected)
    {
        Assert.Equal(expected, PersianDateTime.GetYearEra(year, era));
    }

    [Fact]
    public void GetStandardMonthName_AllMonths_ShouldReturnCorrectNames()
    {
        Assert.Equal("فروردین", PersianDateTime.GetStandardMonthName(1));
        Assert.Equal("اردیبهشت", PersianDateTime.GetStandardMonthName(2));
        Assert.Equal("خرداد", PersianDateTime.GetStandardMonthName(3));
        Assert.Equal("تیر", PersianDateTime.GetStandardMonthName(4));
        Assert.Equal("مرداد", PersianDateTime.GetStandardMonthName(5));
        Assert.Equal("شهریور", PersianDateTime.GetStandardMonthName(6));
        Assert.Equal("مهر", PersianDateTime.GetStandardMonthName(7));
        Assert.Equal("آبان", PersianDateTime.GetStandardMonthName(8));
        Assert.Equal("آذر", PersianDateTime.GetStandardMonthName(9));
        Assert.Equal("دی", PersianDateTime.GetStandardMonthName(10));
        Assert.Equal("بهمن", PersianDateTime.GetStandardMonthName(11));
        Assert.Equal("اسفند", PersianDateTime.GetStandardMonthName(12));
    }

    [Fact]
    public void GetAstronomicalMonthName_Month5_ShouldReturnAsad()
    {
        Assert.Equal("اسد", PersianDateTime.GetAstronomicalMonthName(5));
    }

    [Fact]
    public void GetAstronomicalMonthName_AllMonths_ShouldReturnCorrectNames()
    {
        Assert.Equal("حمل", PersianDateTime.GetAstronomicalMonthName(1));
        Assert.Equal("ثور", PersianDateTime.GetAstronomicalMonthName(2));
        Assert.Equal("جوزا", PersianDateTime.GetAstronomicalMonthName(3));
        Assert.Equal("سرطان", PersianDateTime.GetAstronomicalMonthName(4));
        Assert.Equal("اسد", PersianDateTime.GetAstronomicalMonthName(5));
        Assert.Equal("سنبله", PersianDateTime.GetAstronomicalMonthName(6));
        Assert.Equal("میزان", PersianDateTime.GetAstronomicalMonthName(7));
        Assert.Equal("عقرب", PersianDateTime.GetAstronomicalMonthName(8));
        Assert.Equal("قوس", PersianDateTime.GetAstronomicalMonthName(9));
        Assert.Equal("جدی", PersianDateTime.GetAstronomicalMonthName(10));
        Assert.Equal("دلو", PersianDateTime.GetAstronomicalMonthName(11));
        Assert.Equal("حوت", PersianDateTime.GetAstronomicalMonthName(12));
    }

    [Fact]
    public void GetAstronomicalMonthName_InvalidMonth_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PersianDateTime.GetAstronomicalMonthName(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => PersianDateTime.GetAstronomicalMonthName(13));
    }

    [Fact]
    public void ToLongDateString_WithAstronomicalStyle_ShouldContainAsad()
    {
        var date = new PersianDateTime(1403, 5, 1);
        var result = date.ToLongDateString(MonthNameStyle.Astronomical);
        Assert.Contains("اسد", result);
    }

    [Fact]
    public void ToLongDateString_WithStandardStyle_ShouldContainMordad()
    {
        var date = new PersianDateTime(1403, 5, 1);
        var result = date.ToLongDateString(MonthNameStyle.Standard);
        Assert.Contains("مرداد", result);
    }

    [Fact]
    public void ToLongDateString_Default_ShouldUseStandardSolarHijri()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Equal(date.ToLongDateString(), date.ToLongDateString(MonthNameStyle.Standard, PersianCalendarEra.SolarHijri));
    }

    [Fact]
    public void ToLongDateString_WithShahanshahiEra_ShouldShowCorrectYear()
    {
        var date = new PersianDateTime(1403, 1, 1);
        var result = date.ToLongDateString(MonthNameStyle.Standard, PersianCalendarEra.Shahanshahi);
        Assert.Contains("2583", result);
    }

    [Fact]
    public void YearEra_InvalidEra_ShouldThrow()
    {
        var date = new PersianDateTime(1403, 1, 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => date.YearEra((PersianCalendarEra)99));
    }

    [Fact]
    public void ToUnixTimestamp_RoundTrip_ShouldPreserveValue()
    {
        var original = new PersianDateTime(1403, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var timestamp = original.ToUnixTimestamp();
        var roundTrip = PersianDateTime.FromUnixTimestamp(timestamp);
        Assert.Equal(original, roundTrip);
    }

    [Fact]
    public void ToUnixTimestampMilliseconds_RoundTrip_ShouldPreserveValue()
    {
        var original = new PersianDateTime(1403, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var timestamp = original.ToUnixTimestampMilliseconds();
        var roundTrip = PersianDateTime.FromUnixTimestampMilliseconds(timestamp);
        Assert.Equal(original, roundTrip);
    }

    [Theory]
    [InlineData("1403/01/01 24:00:00")]
    [InlineData("1403/01/01 12:60:00")]
    [InlineData("1403/01/01 12:00:60")]
    [InlineData("1403/01/01 12:xx:00")]
    [InlineData("1403/01/01 12:00")]
    [InlineData("14030101126000")]
    public void TryParse_InvalidTime_ShouldReturnFalse(string value)
    {
        Assert.False(PersianDateTime.TryParse(value, out _));
    }

    [Fact]
    public void ToUnixTimestamp_LocalKind_ShouldMatchDateTimeOffset()
    {
        var localDateTime = new DateTime(2024, 3, 20, 12, 30, 0, DateTimeKind.Local);
        var persian = new PersianDateTime(localDateTime);
        var expected = new DateTimeOffset(localDateTime).ToUnixTimeSeconds();

        Assert.Equal(expected, persian.ToUnixTimestamp());
    }

    [Fact]
    public void FromUnixTimestamp_Zero_ShouldReturnCorrectPersianDate()
    {
        var persian = PersianDateTime.FromUnixTimestamp(0);
        Assert.Equal(1348, persian.Year);
        Assert.Equal(10, persian.Month);
        Assert.Equal(11, persian.Day);
    }

}

