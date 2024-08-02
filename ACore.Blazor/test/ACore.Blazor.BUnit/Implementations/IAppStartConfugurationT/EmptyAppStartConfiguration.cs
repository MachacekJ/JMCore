using System;
using System.Collections.Generic;
using ACore.Blazor.Services.App;
using ACore.Blazor.Services.Page.Implementations;
using ACore.Blazor.Services.Page.Interfaces;
using ACore.Blazor.Services.Page.Models;
using ACore.Localizer.Storage;

namespace ACore.Blazor.BUnit.Implementations.IAppStartConfugurationT;

public class EmptyAppStartConfiguration:IAppStartConfiguration
{
    public string AppName => "BTest";
    public IEnumerable<MenuHierarchyItem> LeftMenuHierarchy => Array.Empty<MenuHierarchyItem>();
    public IEnumerable<IPageData> AllPages => Array.Empty<IPageData>();
    public IPageData HomePage => new PageDataBuilder("bunit", "BUnit").Build();
    public bool IsTest => false;

    public void ApplyTranslations(ILocalizationStorage localizationStorage)
    {
     
    }
}