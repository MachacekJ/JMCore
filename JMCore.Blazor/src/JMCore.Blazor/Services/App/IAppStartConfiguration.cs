using JMCore.Blazor.Services.Page.Interfaces;
using JMCore.Blazor.Services.Page.Models;
using JMCore.Localizer.Storage;

namespace JMCore.Blazor.Services.App;

public interface IAppStartConfiguration
{
    string AppName { get; }
    IEnumerable<MenuHierarchyItem> LeftMenuHierarchy { get; }
    IEnumerable<IPageData> AllPages { get; }
    IPageData HomePage { get; }
    bool IsTest { get; }
    void ApplyTranslations(ILocalizationStorage localizationStorage);
}