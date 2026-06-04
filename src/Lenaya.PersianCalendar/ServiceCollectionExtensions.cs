using Microsoft.Extensions.DependencyInjection;

namespace Lenaya.PersianCalendar;

/// <summary>Extension methods for registering the Persian calendar service with dependency injection.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Adds the Persian calendar service with default options.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddPersianCalendar(this IServiceCollection services)
    {
        services.AddSingleton<IPersianCalendar, PersianCalendarService>();
        return services;
    }

    /// <summary>Adds the Persian calendar service with custom configuration.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configure">An action to configure <see cref="PersianCalendarOptions"/>.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddPersianCalendar(this IServiceCollection services, Action<PersianCalendarOptions> configure)
    {
        services.Configure(configure);
        services.AddSingleton<IPersianCalendar, PersianCalendarService>();
        return services;
    }
}
