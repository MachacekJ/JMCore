using JMCore.Blazor.Components.SvgIcons;
using JMCore.Blazor.Services.App;
using JMCore.Blazor.Services.Page.Implementations;
using JMCore.Blazor.Services.Page.Interfaces;
using JMCore.Blazor.Services.Page.Models;
using JMCoreTest.Blazor.Client.ResX;
using Telerik.SvgIcons;

namespace JMCoreTest.Blazor.Client;

public class AppStartConfiguration(bool isTest) : AppStartConfigurationBase(isTest)
{
    public override string AppName => "UI test";
    public override IPageData HomePage => AppPageList.Home;

    public override IEnumerable<IPageData> AllPages { get; } = new List<IPageData>
    {
        AppPageList.NotFound,
        AppPageList.Home,
        AppPageList.LocalizationHome,
        AppPageList.LocalizationDetail,
        AppPageList.Analytics,
        AppPageList.LargeContent,
        AppPageList.About,
        AppPageList.Report,
        AppPageList.Echo,
        AppPageList.DirectApi
    };

    public override IEnumerable<MenuHierarchyItem> LeftMenuHierarchy { get; } = new List<MenuHierarchyItem>
    {
        new(AppPageList.Home, SvgNationalFlagIcons.CsCz),
        new(AppPageList.LocalizationHome, SvgIcon.User, new List<MenuHierarchyItem>()
        {
            new(AppPageList.LocalizationDetail, SvgIcon.Gear),
        }),
        new("submenuOther", "Other", SvgIcon.Anchor, new List<MenuHierarchyItem>()
        {
            new(AppPageList.Analytics, SvgIcon.Gear),
            new(AppPageList.LargeContent, SvgNationalFlagIcons.EnUs),
            new(AppPageList.PageNotFound),
        }),
        new(AppPageList.DirectApi, SvgIcon.Gear),
       
        new(AppPageList.Echo, SvgIcon.Envelop),
        new("menuReport","Report", SvgIcon.User, new List<MenuHierarchyItem>()
        {
            new(AppPageList.Report, SvgIcon.FileReport)
        }),
        new(AppPageList.About, SvgIcon.InfoCircle)
    };
}

public static class AppPageList
{
    public static readonly IPageData NotFound = PageDataBuilder.BuildNotFoundPage();
    public static readonly IPageData Home = new PageDataBuilder("home", "Home").SetUrl("/").Build();
  
    
    public static readonly IPageData LocalizationHome = new PageDataBuilder("/localization/home", "Localization").SetResX(typeof(ResX_MainLayout), ResX_MainLayout.MenuItem_LocalizationHomeTitle).Build();
    public static readonly IPageData LocalizationDetail = new PageDataBuilder("/localization/detail", "Localization").SetResX(typeof(ResX_MainLayout), ResX_MainLayout.MenuItem_LocalizationDetailTitle).Build();
  
    
    public static readonly IPageData Analytics = new PageDataBuilder("/analytics", "Analytics").Build();
    public static readonly IPageData LargeContent = new PageDataBuilder("/largeContent", "Large content").Build();
    public static readonly IPageData PageNotFound = new PageDataBuilder("/pageNotFound", "Page not found").Build();

    
    public static readonly IPageData About = new PageDataBuilder("/about", "About").Build();
    public static readonly IPageData Echo = new PageDataBuilder("/echo", "Echo").Build();
    public static readonly IPageData Report = new PageDataBuilder("/report", "Report").Build();
    public static readonly IPageData DirectApi = new PageDataBuilder("/directAPI", "Direct API").Build();
}