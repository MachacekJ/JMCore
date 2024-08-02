using System.Resources;
using ACore.Localizer;
using ACore.ResX;

namespace ACore.Server.ResX;

public static class ResXSourceRegister
{
    public static readonly Dictionary<int, string> SupportedCultures = new() { { 1033, "en-US" }, { 1029, "cs-CZ" } };

    public static List<ResXSource> DefaultSourceResx()
    {
        var dic = new List<ResXSource>
        {
            new(
                nameof(ResX_Errors),
                new ResourceManager(typeof(Resources.ResX_Errors)),
                LocalizationScopeEnum.Server | LocalizationScopeEnum.Client),
            new(
                nameof(ResX_DataAnnotation),
                new ResourceManager(typeof(Resources.ResX_DataAnnotation)),
                LocalizationScopeEnum.Server| LocalizationScopeEnum.Client),
            new(
                nameof(ResX_General),
                new ResourceManager(typeof(Resources.ResX_General)),
                LocalizationScopeEnum.Server | LocalizationScopeEnum.Client)
        };

        return dic;
    }
}