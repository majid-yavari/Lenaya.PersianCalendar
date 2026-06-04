using System.Text.Json.Serialization;
using Lenaya.PersianCalendar;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new PersianDateTimeJsonConverter());
    });

builder.Services.AddPersianCalendar(options =>
{
    options.MonthNameStyle = MonthNameStyle.Standard;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
