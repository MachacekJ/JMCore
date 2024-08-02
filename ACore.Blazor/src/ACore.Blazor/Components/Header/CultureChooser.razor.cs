using System.Globalization;
using ACore.Blazor.Services.App.Models;
using Microsoft.AspNetCore.Components.Web;

namespace ACore.Blazor.Components.Header;

public partial class CultureChooser : JMComponentBase
{
    private LanguageItem _value = Config.AvailableLanguage.AllSupportedLanguages.First();

    private static CultureInfo Culture => CultureInfo.CurrentUICulture;

    protected override void OnInitialized()
    {
        var result = Config.AvailableLanguage.AllSupportedLanguages.FirstOrDefault(a => a.Id == Culture.Name);
        if (result == null)
        {
            _value = Config.AvailableLanguage.AllSupportedLanguages.First();
            return;
        }
        _value = result;
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        AppState.ShowRightMenu(RightMenuTypeEnum.Language);
    }
}