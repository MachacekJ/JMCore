using System.Collections;
using System.Globalization;
using JMCore.Localizer;
using JMCore.Server.Storages.Modules.LocalizationModule.Models;

namespace JMCore.Server.ResX.Helpers;

/// <summary>
/// Load all resx files info.
/// </summary>
public static class ResXLoader
{
    public static List<ILocalizationRecord> LoadFromResx(IEnumerable<ResXSource> resources, IEnumerable<int> cultures)
    {
        var all = new List<ILocalizationRecord>();
        var allCultures = cultures as int[] ?? cultures.ToArray();

        foreach (var resx in resources)
        {
            foreach (var lcid in allCultures)
            {
                var culture = new CultureInfo(lcid);

                var resourceSet = resx.ResourceManager.GetResourceSet(culture, true, true);
                if (resourceSet == null)
                    continue;

                foreach (DictionaryEntry entry in resourceSet)
                {
                    var loc = new LocalizationEntity()
                    {
                        ContextId = resx.ContextId,
                        Lcid = culture.LCID,
                        MsgId = entry.Key.ToString()!,
                        Translation = entry.Value == null
                            ? string.Empty
                            : entry.Value.ToString()!,
                        Scope = resx.Scope
                    };
                    all.Add(loc);
                }
            }
        }
        return all;
    }
}