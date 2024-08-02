using ACore.Blazor.Services.Page.Interfaces;
using ACore.Blazor.Services.Page.Models;
using ACore.Localizer.Storage;

namespace ACore.Blazor.Services.App;

public interface IAppStartConfiguration
{
    string AppName { get; }
    IEnumerable<MenuHierarchyItem> LeftMenuHierarchy { get; }
    IEnumerable<IPageData> AllPages { get; }
    IPageData HomePage { get; }
    bool IsTest { get; }
    void ApplyTranslations(ILocalizationStorage localizationStorage);
}