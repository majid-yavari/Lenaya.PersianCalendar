# Lenaya.PersianCalendar

<div dir="ltr">

[![CI](https://img.shields.io/badge/build-pending-lightgrey)](https://github.com/majid-yavari/Lenaya.PersianCalendar/actions)
[![NuGet](https://img.shields.io/nuget/v/Lenaya.PersianCalendar)](https://www.nuget.org/packages/Lenaya.PersianCalendar)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Lenaya.PersianCalendar)](https://www.nuget.org/packages/Lenaya.PersianCalendar)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![Target](https://img.shields.io/badge/net-8.0%20%7c%209.0%20%7c%2010.0-blueviolet)]()

</div>

Lenaya.PersianCalendar is a lightweight .NET library for working with Persian (Solar Hijri / Shamsi) dates and times. It includes conversion between Gregorian and Persian dates, formatting, parsing, arithmetic, holiday helpers, JSON serialization, and dependency injection support.

## Features

- `PersianDateTime`, `PersianDate`, and `PersianTime` value types
- Gregorian/Persian date conversion
- Date and time arithmetic
- Custom formatting such as `yyyy/MM/dd`, `dddd MMMM yyyy`, `HH:mm:ss`, `hh:mm:ss tt`, and `gg`
- Parsing for `yyyy/MM/dd`, `yyyy/MM/dd HH:mm:ss`, compact digits, Persian digits, and common separators
- Standard Persian month names and astronomical month names
- Solar Hijri and Shahanshahi year display
- Fixed and calculated lunar holiday helpers
- `System.Text.Json` converter
- ASP.NET Core dependency injection via `AddPersianCalendar()`
- XML documentation for IntelliSense

## Installation

```bash
dotnet add package Lenaya.PersianCalendar
```

## Quick Start

```csharp
using Lenaya.PersianCalendar;

var now = PersianDateTime.Now;
var date = new PersianDateTime(1403, 1, 1, 14, 30, 0);

Console.WriteLine(date.ToString("yyyy/MM/dd"));
// 1403/01/01

Console.WriteLine(date.ToLongDateString());
// چهارشنبه 1 فروردین 1403

Console.WriteLine(date.ToString("dddd MMMM yyyy - hh:mm:ss tt"));
// چهارشنبه فروردین 1403 - 02:30:00 ب.ظ

var gregorian = date.ToDateTime();
var persian = new PersianDateTime(new DateTime(2024, 3, 20));

Console.WriteLine(persian);
// 1403/01/01
```

## Parsing

```csharp
if (PersianDateTime.TryParse("1403/01/01 14:30:00", out var parsed))
{
    Console.WriteLine(parsed.ToString("yyyy-MM-dd HH:mm:ss"));
}

if (PersianDateTime.TryParse("14030101143000", out var compact))
{
    Console.WriteLine(compact.ToLongDateTimeString());
}

if (PersianDateTime.TryParse("۱۴۰۳-۰۱-۰۱T۱۴:۳۰:۰۰", DateTimeKind.Utc, out var utcParsed))
{
    Console.WriteLine(utcParsed.Kind);
}

var fromText = new PersianDateTime("1405/11/13 22:13:45", DateTimeKind.Utc);
var fromLooseText = new PersianDateTime("۱۴۰۵-۱۱-۱۳ ساعت ۲۲:۱۳:۴۵");
```

Parsing ignores non-digit characters when the remaining digits form either `yyyyMMdd` or `yyyyMMddHHmmss`. That means common separators, text around the date, and Persian digits are supported. Inputs with incomplete digit counts or invalid Persian dates still fail, for example `140511`, `1405/13/01`, and `1405/11/31`.

## SQLite Julian Day

SQLite can store dates as `julianday()` values. You can convert to and from that format:

```csharp
var date = new PersianDateTime(1403, 1, 1, 14, 30, 45, DateTimeKind.Utc);
double julianDay = date.ToSqliteJulianDay();

var roundTrip = PersianDateTime.FromSqliteJulianDay(julianDay, DateTimeKind.Utc);
```

## Age and Relative Time

```csharp
var birth = new PersianDateTime(1366, 5, 22);
var today = new PersianDateTime(1404, 10, 4);

Console.WriteLine(birth.GetAge(today));
// 38

Console.WriteLine(birth.GetAgeString(today, detailed: true));
// 38 سال و 4 ماه و 12 روز

Console.WriteLine(birth.ToRelativeString(today, detailed: true));
// 38 سال و 4 ماه و 12 روز پیش
```

## Dependency Injection

```csharp
using Lenaya.PersianCalendar;

builder.Services.AddPersianCalendar(options =>
{
    options.MonthNameStyle = MonthNameStyle.Standard;
    options.Era = PersianCalendarEra.SolarHijri;
});
```

## JSON Serialization

```csharp
using System.Text.Json;
using Lenaya.PersianCalendar;

var compactOptions = new JsonSerializerOptions();
compactOptions.Converters.Add(new PersianDateTimeJsonConverter());

var json = JsonSerializer.Serialize(new PersianDateTime(1403, 1, 1, 14, 30, 0), compactOptions);
// "14030101143000"

var fullOptions = new JsonSerializerOptions();
fullOptions.Converters.Add(new PersianDateTimeFullJsonConverter());

var fullJson = JsonSerializer.Serialize(
    new PersianDateTime(1403, 1, 1, 14, 30, 0, 123, DateTimeKind.Utc),
    fullOptions);
// {"year":1403,"month":1,"day":1,"hour":14,"minute":30,"second":0,"millisecond":123,"kind":"Utc"}
```

Use `PersianDateTimeJsonConverter` for compact string storage, or `PersianDateTimeFullJsonConverter` when `Millisecond` and `Kind` must round-trip.

## Holidays

```csharp
var holidays = PersianHolidays.GetHolidays(1403);
var isNowruz = PersianHolidays.IsHoliday(new PersianDateTime(1403, 1, 1));
```

## Accuracy Notes

The library uses a fast arithmetic Persian calendar implementation based on the common 33-year leap-year cycle. It matches common contemporary Persian calendar dates used in the tests, but Persian calendar leap-year rules can differ from astronomical or platform-specific implementations for some distant years. If your application needs official long-range civil-calendar accuracy, compare the required year range against your target authority before using it in production.

Lunar holidays are calculated algorithmically and may differ from officially announced dates that depend on moon sighting or government declarations.

## Entity Framework Core

Entity Framework Core support is intentionally not included in the main package right now, so applications that only need calendar/date functionality do not pull EF Core as a dependency. EF support can be added later as a separate package.

## Build, Test, and Pack

```bash
dotnet build -c Release
dotnet test
dotnet pack src/Lenaya.PersianCalendar/Lenaya.PersianCalendar.csproj -c Release
```

## Benchmarks

```bash
dotnet run -c Release --project benchmarks/Lenaya.PersianCalendar.Benchmarks -- --job Short
```

BenchmarkDotNet output is generated under `BenchmarkDotNet.Artifacts/` and is ignored by Git.

## Target Frameworks

- `net8.0`
- `net9.0`
- `net10.0`

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE).
