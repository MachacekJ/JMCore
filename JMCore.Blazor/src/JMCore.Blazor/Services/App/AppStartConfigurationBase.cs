using JMCore.Blazor.Services.Page.Models;
using JMCore.Localizer.Storage;
using System.Globalization;
using JMCore.Blazor.Services.Page.Interfaces;

namespace JMCore.Blazor.Services.App;

public abstract class AppStartConfigurationBase(bool isTest) : IAppStartConfiguration
{
    public abstract string AppName { get; }
    public abstract IEnumerable<MenuHierarchyItem> LeftMenuHierarchy { get; }
    public abstract IEnumerable<IPageData> AllPages { get; }
    public abstract IPageData HomePage { get; }
    public bool IsTest { get; } = isTest;

    public void ApplyTranslations(ILocalizationStorage localizationStorage)
    {
        foreach (var pageData in AllPages)
        {
            if (pageData.ResX.HasValue)
                pageData.Title = GetLocalizedTitle(localizationStorage, pageData.ResX.Value);
        }

        foreach (var menuHierarchyItem in LeftMenuHierarchy)
        {
            HierarchyRecurrence(localizationStorage, menuHierarchyItem);
        }
    }
    
    private static void HierarchyRecurrence(ILocalizationStorage localizationStorage, MenuHierarchyItem menuHierarchyItem)
    {
        if (menuHierarchyItem.ResX.HasValue)
            menuHierarchyItem.Title = GetLocalizedTitle(localizationStorage, menuHierarchyItem.ResX.Value);

        foreach (var hierarchyItem in menuHierarchyItem.Children)
        {
            HierarchyRecurrence(localizationStorage, hierarchyItem);
        }
    }

    private static string GetLocalizedTitle(ILocalizationStorage localizationStorage, (Type Type, string Name) resX)
    {
        var lcid = CultureInfo.DefaultThreadCurrentUICulture == null ? 1033 : CultureInfo.DefaultThreadCurrentUICulture.LCID;
        var translation = localizationStorage.All
            .FirstOrDefault(i => i.Lcid == lcid
                                 && i.ContextId == resX.Type.Name
                                 && i.MsgId == resX.Name);
        return translation != null ? translation.Translation : $"{lcid}-{resX.Type.Name}-{resX.Name}";

    }
}