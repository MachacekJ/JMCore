using ACore.Localizer.Storage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace ACore.Localizer;

public class StringLocalizerFactory(ILocalizationStorage storage, IOptions<LocalizationOptions> localizationOptions) : IStringLocalizerFactory
{
    IStringLocalizer IStringLocalizerFactory.Create(Type resourceSource)
    {
        var contextId = resourceSource.Name;

        return new StringLocalizer(storage, contextId, localizationOptions);
    }

    IStringLocalizer IStringLocalizerFactory.Create(string baseName, string location)
    {
        throw new NotImplementedException();
        //return new StringLocalizer(storage, baseName + location, localizationOptions);
    }
}