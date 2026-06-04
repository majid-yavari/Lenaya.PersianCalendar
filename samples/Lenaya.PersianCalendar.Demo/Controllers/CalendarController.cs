using Microsoft.AspNetCore.Mvc;

namespace Lenaya.PersianCalendar.Demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalendarController : ControllerBase
{
    private readonly IPersianCalendar _calendar;

    public CalendarController(IPersianCalendar calendar)
    {
        _calendar = calendar;
    }

    [HttpGet("today")]
    public IActionResult Today()
    {
        var today = _calendar.Today;
        return Ok(new
        {
            persianDate = today.ToString("yyyy/MM/dd"),
            persianDateWithNames = _calendar.Format(today),
            dayOfWeek = today.DayOfWeek.ToString(),
            dayOfYear = today.DayOfYear,
            weekNumber = today.WeekNumber,
            isLeapYear = _calendar.IsLeapYear(today.Year)
        });
    }

    [HttpGet("now")]
    public IActionResult Now()
    {
        var now = _calendar.Now;
        return Ok(new
        {
            persianDateTime = now.ToString("yyyy/MM/dd HH:mm:ss"),
            persianDateTimeWithNames = _calendar.Format(now),
            kind = now.Kind.ToString(),
            timeOfDay = now.TimeOfDay.ToString()
        });
    }

    [HttpGet("utc-now")]
    public IActionResult UtcNow()
    {
        var utcNow = PersianDateTime.UtcNow;
        return Ok(new
        {
            utcNow = utcNow.ToString("yyyy/MM/dd HH:mm:ss"),
            kind = utcNow.Kind.ToString(),
            toLocal = utcNow.ToLocalDateTime().ToString("yyyy/MM/dd HH:mm:ss"),
            unixTimestamp = utcNow.ToUnixTimestamp()
        });
    }

    [HttpGet("format")]
    public IActionResult Format(
        [FromQuery] int year = 1403,
        [FromQuery] int month = 6,
        [FromQuery] int day = 15,
        [FromQuery] int hour = 14,
        [FromQuery] int minute = 5,
        [FromQuery] int second = 3,
        [FromQuery] string? format = null)
    {
        var date = new PersianDateTime(year, month, day, hour, minute, second);
        var results = new Dictionary<string, string>
        {
            ["yyyy/MM/dd"] = date.ToString("yyyy/MM/dd"),
            ["yyyy-MM-dd"] = date.ToString("yyyy-MM-dd"),
            ["dddd MMMM yyyy"] = date.ToString("dddd MMMM yyyy"),
            ["HH:mm:ss"] = date.ToString("HH:mm:ss"),
            ["hh:mm:ss tt"] = date.ToString("hh:mm:ss tt"),
            ["gg"] = date.ToString("gg"),
            ["ss.fff"] = date.ToString("ss.fff"),
            ["Custom"] = format is not null ? date.ToString(format) : "not provided"
        };
        if (_calendar.Format(date) != date.ToString("yyyy/MM/dd"))
        {
            results["ServiceFormat"] = _calendar.Format(date);
        }
        return Ok(results);
    }

    [HttpGet("properties")]
    public IActionResult Properties([FromQuery] int year = 1403, [FromQuery] int month = 6, [FromQuery] int day = 15)
    {
        var date = new PersianDateTime(year, month, day);
        return Ok(new
        {
            year = date.Year,
            month = date.Month,
            day = date.Day,
            dayOfWeek = date.DayOfWeek.ToString(),
            dayOfYear = date.DayOfYear,
            weekNumber = date.WeekNumber,
            daysInMonth = _calendar.DaysInMonth(year, month),
            daysInYear = _calendar.DaysInYear(year),
            isLeapYear = _calendar.IsLeapYear(year),
            isValid = _calendar.IsValidDate(year, month, day),
            isWeekend = date.IsWeekend(),
            startOfMonth = date.StartOfMonth().ToString("yyyy/MM/dd"),
            endOfMonth = date.EndOfMonth().ToString("yyyy/MM/dd"),
            startOfYear = date.StartOfYear().ToString("yyyy/MM/dd"),
            endOfYear = date.EndOfYear().ToString("yyyy/MM/dd"),
            daysInThisMonth = date.DaysInThisMonth(),
            daysInThisYear = date.DaysInThisYear()
        });
    }

    [HttpGet("convert")]
    public IActionResult Convert([FromQuery] int year = 1403, [FromQuery] int month = 1, [FromQuery] int day = 1)
    {
        var persian = new PersianDateTime(year, month, day);
        var gregorian = persian.ToDateTime();
        return Ok(new
        {
            persian = persian.ToString("yyyy/MM/dd"),
            gregorian = gregorian.ToString("yyyy-MM-dd"),
            dayOfWeek = persian.DayOfWeek.ToString(),
            fromService = _calendar.FromDateTime(gregorian).ToString("yyyy/MM/dd")
        });
    }

    [HttpGet("diff")]
    public IActionResult Diff(
        [FromQuery] int y1 = 1403, [FromQuery] int m1 = 1, [FromQuery] int d1 = 1,
        [FromQuery] int y2 = 1403, [FromQuery] int m2 = 12, [FromQuery] int d2 = 29)
    {
        var a = new PersianDateTime(y1, m1, d1);
        var b = new PersianDateTime(y2, m2, d2);
        return Ok(new
        {
            from = a.ToString("yyyy/MM/dd"),
            to = b.ToString("yyyy/MM/dd"),
            days = b - a,
            timeSpan = b.Subtract(a).ToString(@"dd\.hh\:mm"),
            min = PersianDateTime.Min(a, b).ToString("yyyy/MM/dd"),
            max = PersianDateTime.Max(a, b).ToString("yyyy/MM/dd")
        });
    }

    [HttpGet("arithmetic")]
    public IActionResult Arithmetic(
        [FromQuery] int year = 1403, [FromQuery] int month = 6, [FromQuery] int day = 15,
        [FromQuery] int hour = 10, [FromQuery] int minute = 30, [FromQuery] int second = 0,
        [FromQuery] int days = 10, [FromQuery] int months = 3, [FromQuery] int years = 1)
    {
        var date = new PersianDateTime(year, month, day, hour, minute, second);
        return Ok(new
        {
            original = date.ToString("yyyy/MM/dd HH:mm:ss"),
            addDays = date.AddDays(days).ToString("yyyy/MM/dd"),
            addMonths = date.AddMonths(months).ToString("yyyy/MM/dd"),
            addYears = date.AddYears(years).ToString("yyyy/MM/dd"),
            addHours = date.AddHours(2.5).ToString("yyyy/MM/dd HH:mm:ss"),
            addMinutes = date.AddMinutes(90).ToString("yyyy/MM/dd HH:mm:ss"),
            addSeconds = date.AddSeconds(3600).ToString("yyyy/MM/dd HH:mm:ss")
        });
    }

    [HttpGet("deconstruct")]
    public IActionResult Deconstruct([FromQuery] int year = 1403, [FromQuery] int month = 6, [FromQuery] int day = 15)
    {
        var (y, m, d) = new PersianDateTime(year, month, day);
        return Ok(new { year = y, month = m, day = d, message = $"Deconstructed: year={y}, month={m}, day={d}" });
    }

    [HttpGet("parse")]
    public IActionResult Parse([FromQuery] string value = "1403/12/15")
    {
        if (PersianDateTime.TryParse(value, out var result))
        {
            return Ok(new
            {
                input = value,
                parsed = result.ToString("yyyy/MM/dd"),
                dayOfWeek = result.DayOfWeek.ToString(),
                serviceFormat = _calendar.Format(result)
            });
        }
        return BadRequest(new { error = $"Cannot parse '{value}'" });
    }

    [HttpGet("persian-date")]
    public IActionResult PersianDate([FromQuery] int year = 1403, [FromQuery] int month = 12, [FromQuery] int day = 29)
    {
        var date = new PersianDate(year, month, day);
        return Ok(new
        {
            persianDate = date.ToString(),
            dayOfWeek = date.DayOfWeek.ToString(),
            dayOfYear = date.DayOfYear,
            weekNumber = date.WeekNumber,
            isWeekend = date.IsWeekend(),
            addDays = date.AddDays(3).ToString(),
            startOfMonth = date.StartOfMonth().ToString(),
            endOfMonth = date.EndOfMonth().ToString()
        });
    }

    [HttpGet("persian-time")]
    public IActionResult PersianTime([FromQuery] int hour = 14, [FromQuery] int minute = 30,
        [FromQuery] int second = 15, [FromQuery] int millisecond = 120)
    {
        var time = new PersianTime(hour, minute, second, millisecond);
        return Ok(new
        {
            persianTime = time.ToString(),
            formatHHmmss = time.ToString("HH:mm:ss"),
            formatHhmmssTt = time.ToString("hh:mm:ss tt"),
            isBetween = time.IsBetween(new PersianTime(14, 0), new PersianTime(15, 0)),
            addHoursWrap = time.AddHours(10).ToString(),
            addSeconds = time.AddSeconds(3600).ToString()
        });
    }

    [HttpGet("holidays")]
    public IActionResult Holidays([FromQuery] int year = 1403)
    {
        var holidays = PersianHolidays.GetHolidays(year);
        return Ok(new
        {
            year,
            count = holidays.Count,
            items = holidays.Select(h => new
            {
                name = h.Name,
                date = h.Date.ToString("yyyy/MM/dd"),
                type = h.Type.ToString()
            })
        });
    }

    [HttpGet("month/{year}/{month}")]
    public IActionResult Month(int year, int month)
    {
        var m = new PersianMonth(year, month);
        return Ok(new
        {
            year = m.Year,
            month = m.Month,
            name = PersianDateTime.GetStandardMonthName(month),
            astronomicalName = PersianDateTime.GetAstronomicalMonthName(month),
            dayCount = m.DayCount,
            weeksCount = m.WeeksCount,
            firstDayOfWeek = m.FirstDayOfWeek.ToString(),
            startDate = new PersianDate(m.Year, m.Month, 1).ToString(),
            endDate = new PersianDate(m.Year, m.Month, m.DayCount).ToString()
        });
    }

    [HttpGet("age")]
    public IActionResult Age([FromQuery] int year = 1366, [FromQuery] int month = 5, [FromQuery] int day = 22)
    {
        var birth = new PersianDateTime(year, month, day);
        return Ok(new
        {
            birthDate = birth.ToString("yyyy/MM/dd"),
            age = birth.GetAge(),
            relative = birth.ToRelativeString()
        });
    }
}
