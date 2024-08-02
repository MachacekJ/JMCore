using ACore.Blazor.Components.SvgIcons;
using ACore.Blazor.Services.App.Models;

namespace ACore.Blazor.Config;

public static class AvailableLanguage
{
    public static IEnumerable<LanguageItem> AllSupportedLanguages { get; } = new[] {
        new LanguageItem (1033,"en-US", "English", SvgNationalFlagIcons.EnUs ),
        new LanguageItem ( 1029, "cs-CZ", "Čeština", SvgNationalFlagIcons.CsCz ) };
}