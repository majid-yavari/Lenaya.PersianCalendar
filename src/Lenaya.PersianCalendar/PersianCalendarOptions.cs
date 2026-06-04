namespace Lenaya.PersianCalendar;

/// <summary>Configuration options for the Persian calendar service.</summary>
public class PersianCalendarOptions
{
    /// <summary>Gets or sets the calendar era to use when formatting.</summary>
    public PersianCalendarEra Era { get; set; } = PersianCalendarEra.SolarHijri;

    /// <summary>Gets or sets the style of month names to display.</summary>
    public MonthNameStyle MonthNameStyle { get; set; } = MonthNameStyle.Standard;
}
