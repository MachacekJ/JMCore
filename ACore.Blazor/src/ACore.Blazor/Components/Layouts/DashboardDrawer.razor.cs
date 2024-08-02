using ACore.Blazor.Services.App;
using ACore.Blazor.Services.App.Models;
using ACore.Blazor.Services.Page.Interfaces;
using ACore.Blazor.Services.Page.Models;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace ACore.Blazor.Components.Layouts;

public partial class DashboardDrawer : JMComponentBase, IDisposable
{
    private string _pageTitle = string.Empty;
    private IEnumerable<BreadcrumbItem> _items = new List<BreadcrumbItem>();
    private bool _expanded = true;
    private TelerikDrawer<int> _menuDrawer = null!;
    private TelerikDrawer<int> _menuDrawer2 = null!;
    private DrawerMode _mode = DrawerMode.Push;
    private List<int> _fakeData = new() { 0 };
    private List<int> _fakeData2 = new() { 0 };
    //[Parameter] public RenderFragment? TopBar { get; set; }

    [Parameter] public RenderFragment? Body { get; set; }

    [Inject] public IAppStartConfiguration AppSettings { get; set; } = null!;

    [Inject] public IPageManager PageManager { get; set; } = null!;

    public void Dispose()
    {
        AppState.OnPageChangeAsync -= AppStateOnPageChangeAsync;
    }

    protected override void OnInitialized()
    {
        AppState.OnPageChangeAsync += AppStateOnPageChangeAsync;
        AppState.OnShowRightMenuAsync += ShowRightMenuAsync;
        _pageTitle = AppState.PageData.Title;
    }

    protected override async Task OnInitializedAsync()
    {
        _items = await PageManager.GetBreadcrumbsForPageAsync(AppState.PageData);
        await base.OnInitializedAsync();
    }

    private async Task AppStateOnPageChangeAsync(IPageData pageData)
    {
        if (_pageTitle == pageData.PageId)
            return;

        _pageTitle = pageData.PageId;
        _items = await PageManager.GetBreadcrumbsForPageAsync(pageData);
        if (AppState.ResponsiveType == ResponsiveTypeEnum.Mobile)
            await _menuDrawer.CollapseAsync();

        StateHasChanged();
    }

    private async Task ToggleMenuDrawer()
    {
        if (_menuDrawer.Expanded)
            await _menuDrawer.CollapseAsync();
        else
            await _menuDrawer.ExpandAsync();
    }

    private void ExpandedChangedHandler(bool expanded)
    {
        _expanded = expanded;
    }

    private async Task MediaQueryChange(bool isSmall)
    {
        AppState.SetResponsiveType(isSmall ? ResponsiveTypeEnum.Mobile : ResponsiveTypeEnum.Desktop);
        if (AppState.ResponsiveType == ResponsiveTypeEnum.Mobile)
        {
            _mode = DrawerMode.Overlay;
            await _menuDrawer.CollapseAsync();
        }
        else
        {
            _mode = DrawerMode.Push;
            await _menuDrawer.ExpandAsync();
        }
    }

    private async Task ShowRightMenuAsync(RightMenuTypeEnum rightMenuType)
    {
        await _menuDrawer2.ExpandAsync();
    }
}