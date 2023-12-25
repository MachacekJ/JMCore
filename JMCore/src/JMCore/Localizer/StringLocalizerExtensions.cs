using JMCore.Localizer.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace JMCore.Localizer;

public static class StringLocalizerExtensions
{
    public static IServiceCollection AddStringMemoryLocalization(
        this IServiceCollection services,
        Action<LocalizationOptions> setupAction)
    {
        services.Configure(setupAction);
        services.AddSingleton<ILocalizationStorage, LocalizationMemoryStorage>();
        services.TryAddSingleton<IStringLocalizerFactory, StringLocalizerFactory>();
        services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
        return services;
    }
}