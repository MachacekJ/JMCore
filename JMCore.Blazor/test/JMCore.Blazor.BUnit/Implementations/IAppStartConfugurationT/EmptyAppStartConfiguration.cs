using System;
using System.Collections.Generic;
using JMCore.Blazor.Services.App;
using JMCore.Blazor.Services.Page.Implementations;
using JMCore.Blazor.Services.Page.Interfaces;
using JMCore.Blazor.Services.Page.Models;
using JMCore.Localizer.Storage;

namespace JMCore.Blazor.BUnit.Implementations.IAppStartConfugurationT;

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