using ACore.Blazor.Services.App.Models;
using ACore.Blazor.Services.Page.Implementations;
using ACore.Blazor.Services.Page.Interfaces;
using Microsoft.Extensions.Logging;

namespace ACore.Blazor.Services.App;

public class AppState(IAppStartConfiguration appSettings, ILogger<AppState> logger) : IAppState
{
    //public event Action<IPageData>? OnPageChange;
    public event Func<IPageData, Task>? OnPageChangeAsync;
    public event Func<ResponsiveTypeEnum, Task>? OnResponsiveChange;
    public event Func<RightMenuTypeEnum, Task>? OnShowRightMenuAsync;

    public IPageData PageData { get; private set; } = appSettings.HomePage;

    public IAppStartConfiguration AppSetting { get; } = appSettings;

    private PageStateEnum PageState { get; set; } = PageStateEnum.Initialize;

    public ResponsiveTypeEnum ResponsiveType { get; private set; } = ResponsiveTypeEnum.Desktop;


    public void SetPageState(PageStateEnum pageState)
    {
        PageState = pageState;
    }

    public void SetPage(string path)
    {
        IPageData page;
        if (path is "" or "/")
            page = AppSetting.HomePage;
        else
        {
            var url = path.ToLower();

            if (url.Contains("?"))
                url = url.Remove(url.IndexOf('?'));

            if (url == "_framework/debug/ws-proxy")
                return;

            if (url.StartsWith("/"))
                url = url.Substring(1);

            var foundPage = AppSetting.AllPages.FirstOrDefault(appSettingsPage => url.StartsWith(appSettingsPage.PageId));
            if (foundPage == null)
            {
                logger.LogWarning("Page for path '{path}' has not been found.", path);
                foundPage = AppSetting.AllPages.First(p => p.PageId == PageDataBuilder.PageNotFoundId);
            }

            page = foundPage;
        }

        if (PageData.PageId == page.PageId)
            return;

        PageData = page;

        if (PageState != PageStateEnum.Rendered)
            return;

        OnPageChangeAsync?.Invoke(PageData);
    }

    public void SetResponsiveType(ResponsiveTypeEnum responsiveType)
    {
        if (ResponsiveType == responsiveType)
            return;

        ResponsiveType = responsiveType;

        if (PageState != PageStateEnum.Rendered)
            return;

        OnResponsiveChange?.Invoke(ResponsiveType);
    }

    public void ShowRightMenu(RightMenuTypeEnum rightMenuType)
    {
        OnShowRightMenuAsync?.Invoke(rightMenuType);
    }
}