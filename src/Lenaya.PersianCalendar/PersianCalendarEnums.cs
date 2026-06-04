namespace Lenaya.PersianCalendar;

/// <summary>
/// Specifies the era used in the Persian calendar.
/// </summary>
public enum PersianCalendarEra
{
    /// <summary>
    /// The Solar Hijri era (standard Iranian calendar).
    /// </summary>
    SolarHijri = 0,
    /// <summary>
    /// The Shahanshahi (Imperial) era.
    /// </summary>
    Shahanshahi = 1
}

/// <summary>
/// Specifies the naming style for Persian month names.
/// </summary>
public enum MonthNameStyle
{
    /// <summary>
    /// Uses standard month names.
    /// </summary>
    Standard = 0,
    /// <summary>
    /// Uses astronomical month names.
    /// </summary>
    Astronomical = 1
}
