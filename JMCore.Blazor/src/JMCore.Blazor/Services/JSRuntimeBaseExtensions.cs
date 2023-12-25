using System.Text.RegularExpressions;
using Microsoft.JSInterop;

namespace JMCore.Blazor.Services;

/// <summary>
/// Wrap function from www/js/interop.js
/// </summary>
// ReSharper disable once InconsistentNaming
public static class JSRuntimeBaseExtensions
{
    private static readonly Regex RegExAspNetCoreCulture =
        new(@"c=(?<culture>[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8}))\|uic=(?<uiculture>[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8}))");

    /// <summary>
    /// Return set culture from cookie. eg. en-US, cs-CZ etc.
    /// </summary>
    public static async Task<string?> GetAspNetCoreCultureCookie(this IJSRuntime js)
    {
        var culture = await js.InvokeAsync<string>("cookieStorage.get", ".AspNetCore.Culture");

        return RegExAspNetCoreCulture.IsMatch(culture)
            ? RegExAspNetCoreCulture.Match(culture).Groups["uiculture"].Value
            : null;
    }

    /// <summary>
    /// Set culture into cookie. eg. en-US, cs-CZ etc.
    /// </summary>
    public static async Task SetAspNetCoreCultureCookie(this IJSRuntime js, string culture)
    {
        var escapedCulture = Uri.EscapeDataString($"c={culture}|uic={culture}");
        await js.InvokeVoidAsync("cookieStorage.set",
            $".AspNetCore.Culture={escapedCulture}; max-age=2592000;path=/");
    }
}