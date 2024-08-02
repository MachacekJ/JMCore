using ACore.Blazor.Services.App.Models;
using ACore.Blazor.Services.Page.Interfaces;

namespace ACore.Blazor.Services.App;

public interface IAppState
{
    // event Action? OnPageChange;
    event Func<IPageData, Task> OnPageChangeAsync;
    event Func<ResponsiveTypeEnum, Task> OnResponsiveChange;
    event Func<RightMenuTypeEnum, Task> OnShowRightMenuAsync;

    IPageData PageData { get; }
    ResponsiveTypeEnum ResponsiveType { get; }
    IAppStartConfiguration AppSetting { get; }

    void SetPageState(PageStateEnum pageState);
    void SetPage(string path);
    void SetResponsiveType(ResponsiveTypeEnum responsiveType);
    void ShowRightMenu(RightMenuTypeEnum rightMenuType);
}