using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar.Tests;

public class PersianCalendarServiceTests
{
    [Fact]
    public void AddPersianCalendar_ShouldRegisterService()
    {
        var services = new ServiceCollection();
        services.AddPersianCalendar();
        var provider = services.BuildServiceProvider();

        var service = provider.GetService<IPersianCalendar>();
        Assert.NotNull(service);
    }

    [Fact]
    public void AddPersianCalendar_WithOptions_ShouldRegisterService()
    {
        var services = new ServiceCollection();
        services.AddPersianCalendar(options =>
        {
            options.MonthNameStyle = MonthNameStyle.Astronomical;
        });
        var provider = services.BuildServiceProvider();

        var service = provider.GetService<IPersianCalendar>();
        Assert.NotNull(service);
    }

    [Fact]
    public void PersianCalendarService_Today_ShouldMatchStatic()
    {
        var service = new PersianCalendarService();
        Assert.Equal(PersianDateTime.Today, service.Today);
    }

    [Fact]
    public void PersianCalendarService_Now_ShouldMatchStatic()
    {
        var service = new PersianCalendarService();
        Assert.Equal(PersianDateTime.Now.Hour, service.Now.Hour);
        Assert.Equal(PersianDateTime.Now.Minute, service.Now.Minute);
    }

    [Fact]
    public void PersianCalendarService_FromDateTime_ShouldConvert()
    {
        var service = new PersianCalendarService();
        var dt = new DateTime(2024, 3, 20);
        var result = service.FromDateTime(dt);
        Assert.Equal(new PersianDateTime(1403, 1, 1), result);
    }

    [Fact]
    public void PersianCalendarService_ToDateTime_ShouldConvert()
    {
        var service = new PersianCalendarService();
        var persian = new PersianDateTime(1403, 1, 1);
        var result = service.ToDateTime(persian);
        Assert.Equal(new DateTime(2024, 3, 20), result);
    }

    [Fact]
    public void PersianCalendarService_IsLeapYear_ShouldMatchStatic()
    {
        var service = new PersianCalendarService();
        Assert.Equal(PersianDateTime.IsLeapYear(1403), service.IsLeapYear(1403));
        Assert.Equal(PersianDateTime.IsLeapYear(1404), service.IsLeapYear(1404));
    }

    [Fact]
    public void PersianCalendarService_DaysInMonth_ShouldMatchStatic()
    {
        var service = new PersianCalendarService();
        Assert.Equal(PersianDateTime.DaysInMonth(1403, 12), service.DaysInMonth(1403, 12));
    }

    [Fact]
    public void PersianCalendarService_IsValidDate_ShouldMatchStatic()
    {
        var service = new PersianCalendarService();
        Assert.True(service.IsValidDate(1403, 12, 30));
        Assert.False(service.IsValidDate(1404, 12, 30));
    }

    [Fact]
    public void AddPersianCalendar_ShouldBeSingleton()
    {
        var services = new ServiceCollection();
        services.AddPersianCalendar();
        var provider = services.BuildServiceProvider();

        var instance1 = provider.GetService<IPersianCalendar>();
        var instance2 = provider.GetService<IPersianCalendar>();
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void Format_WithEraShahanshahi_ShouldShowImperialYear()
    {
        var services = new ServiceCollection();
        services.AddPersianCalendar(options =>
        {
            options.Era = PersianCalendarEra.Shahanshahi;
        });
        var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<IPersianCalendar>();

        var date = new PersianDateTime(1403, 1, 1);
        var result = service.Format(date);
        Assert.Contains("2583", result);
    }

    [Fact]
    public void Format_WithAstronomicalMonthNames_ShouldShowAsad()
    {
        var services = new ServiceCollection();
        services.AddPersianCalendar(options =>
        {
            options.MonthNameStyle = MonthNameStyle.Astronomical;
        });
        var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<IPersianCalendar>();

        var date = new PersianDateTime(1403, 5, 1);
        var result = service.Format(date);
        Assert.Contains("اسد", result);
    }

    [Fact]
    public void Format_WithShahanshahiAndAstronomical_ShouldCombineBoth()
    {
        var services = new ServiceCollection();
        services.AddPersianCalendar(options =>
        {
            options.Era = PersianCalendarEra.Shahanshahi;
            options.MonthNameStyle = MonthNameStyle.Astronomical;
        });
        var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<IPersianCalendar>();

        var date = new PersianDateTime(1403, 5, 1);
        var result = service.Format(date);
        Assert.Contains("اسد", result);
        Assert.Contains("2583", result);
    }

    [Fact]
    public void Format_WithTime_ShouldIncludeTime()
    {
        var services = new ServiceCollection();
        services.AddPersianCalendar();
        var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<IPersianCalendar>();

        var date = new PersianDateTime(1403, 1, 1, 14, 30, 0);
        var result = service.Format(date);
        Assert.Contains("14:30:00", result);
        Assert.Contains("ساعت", result);
    }
}
