using JMCore.Blazor.CQRS.LocalStorage;
using JMCore.Blazor.CQRS.LocalStorage.Models;
using JMCore.Blazor.Services.App;
using JMCore.Blazor.Services.Page.Interfaces;
using JMCore.Blazor.Services.Page.Models;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Common.Trees.Models;
using Telerik.Blazor.Components.PanelBar.Models;

namespace JMCore.Blazor.Components.SideBar.LeftSideBar;

public partial class LeftMenuContent : JMComponentBase, IDisposable
{
    private List<PanelBarItem> _rootItems = [];

    private IEnumerable<object> _expandedItems = new List<PanelBarItem>();

    private List<string> _allExp = [];

    [Inject]
    private IAppStartConfiguration AppStartConfiguration { get; set; } = null!;

    protected override void OnInitialized()
    {
        AppState.OnPageChangeAsync += AppState_OnChangeAsync;
    }

    protected override async Task OnInitializedAsync()
    {
        var expanded = new List<PanelBarItem>();
        var memoryExpanded = new List<string>();
        var expandedHistory = await Mediator.Send(new LocalStorageGetQuery(LocalStorageCategoryEnum.AppSettings, nameof(LeftMenuContent)));
        if (expandedHistory.IsValue)
            memoryExpanded = expandedHistory.GetValue<List<string>>() ?? [];

        var rootItems = AppStartConfiguration.LeftMenuHierarchy.Select(menuItem => LoadPanelBarItems(menuItem, memoryExpanded, expanded)).ToList();
        _expandedItems = expanded;
        _rootItems = rootItems;
    }

    private Task OnExpand(PanelBarExpandEventArgs item)
    {
        var id = ((PanelBarItem)item.Item).Id.ToString() ?? throw new Exception("");
        if (!_allExp.Contains(id))
            _allExp.Add(id);
        return Mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.AppSettings, nameof(LeftMenuContent),
            _allExp.ToList(), _allExp.GetType()));
    }

    private Task OnCollapse(PanelBarCollapseEventArgs item)
    {
        var id = ((PanelBarItem)item.Item).Id.ToString() ?? throw new Exception("");
        if (_allExp.Contains(id))
            _allExp.Remove(id);
        return Mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.AppSettings, nameof(LeftMenuContent),
            _allExp.ToList(), _allExp.GetType()));
    }

    private void ExpandedItemsChanged(IEnumerable<object> expandedItems)
    {
        _expandedItems = expandedItems;
    }

    private Task AppState_OnChangeAsync(IPageData pageData)
    {
        StateHasChanged();
        return Task.CompletedTask;
    }

    private void SelectActivePage(PanelBarItemRenderEventArgs itemRender)
    {
        itemRender.Class = null;
        if (itemRender.Item is not PanelBarItem panelBar)
            return;

        if (panelBar.Id == null)
            return;

        if (panelBar.Id.ToString() == AppState.PageData.PageId)
        {
            itemRender.Class = "k-level-selected";
        }
    }

    private PanelBarItem LoadPanelBarItems(MenuHierarchyItem hierarchyItem, List<string> expandedHistory, List<PanelBarItem> expanded)
    {
        var item = hierarchyItem.ToPanelBarItem();

        if (!hierarchyItem.Children.Any())
            return item;

        item.Items = new List<TreeItem>();
        item.HasChildren = true;
        if (expandedHistory.Contains(item.Id.ToString()!))
        {
            expanded.Add(item);
            _allExp.Add(item.Id.ToString()!);
            item.Expanded = true;
        }

        foreach (var children in hierarchyItem.Children)
        {
            var subItem = LoadPanelBarItems(children, expandedHistory, expanded);
            item.Items.Add(subItem);
        }

        return item;
    }

    public void Dispose()
    {
        AppState.OnPageChangeAsync -= AppState_OnChangeAsync;
    }
}