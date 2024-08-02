using System.Globalization;
using ACore.Localizer.Storage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace ACore.Localizer;

/// <summary>
/// Implementation of <see cref="IStringLocalizer"/>
/// </summary>
public class StringLocalizer(ILocalizationStorage storage, string contextId, IOptions<LocalizationOptions> localizationOptions) : IStringLocalizer
{
    private ILocalizationStorage Storage { get; } = storage;
    private readonly LocalizationOptions _localizationOptions = localizationOptions.Value;

    public LocalizedString this[string name] => Localize(name, null);

    public LocalizedString this[string name, params object[] arguments] => Localize(name, arguments);

    private string ContextId { get; set; } = contextId;

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => Storage.All.Select(item => Localize(item.MsgId, null));

    private LocalizedString Localize(string name, object[]? args)
    {
        var res = Storage.All
            .FirstOrDefault(a => a.MsgId == name && a.ContextId == ContextId && a.Lcid == CultureInfo.CurrentUICulture.LCID);
        if (res == null)
            return _localizationOptions.ReturnOnlyKeyIfNotFound
                ? new LocalizedString(name, name, true)
                : new LocalizedString(name, ContextId + ":" + name, true);

        var translated = res.Translation;
        if (args != null && args.Any())
        {
            translated = string.Format(translated, args);
        }

        return new LocalizedString(name, translated);
    }


}