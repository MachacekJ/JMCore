using ACore.Blazor.Services.App;
using ACore.Blazor.Services.Page.Interfaces;
using ACore.Blazor.Services.Page.Models;
using Microsoft.Extensions.Logging;
using Telerik.SvgIcons;

namespace ACore.Blazor.Services.Page.Implementations;

public class PageManager : IPageManager
{
    private readonly IAppStartConfiguration _appSettings;
    private readonly List<MenuHierarchyItem> _leftMenuFlatItems = [];
    private readonly ILogger<PageManager> _logger;

    public PageManager(IAppStartConfiguration appSettings, ILogger<PageManager> logger)
    {
        _appSettings = appSettings;
        _logger = logger;
        ReBuildLeftMenu();
    }

    public Task<IEnumerable<BreadcrumbItem>> GetBreadcrumbsForPageAsync(IPageData page)
    {
        var homePage = _appSettings.HomePage;
        var res = new List<BreadcrumbItem>
        {
            new()
            {
                Text = homePage.Title,
                Title = homePage.Title,
                Icon = SvgIcon.Home,
                Url = homePage.PageId
            }
        };
        if (page.PageId == homePage.PageId)
            return Task.FromResult<IEnumerable<BreadcrumbItem>>(res);
        
        var find = FindMenuItem(_leftMenuFlatItems, page);
        if (find == null)
        {
            _logger.LogError("Page id '{PageUrl}' and title '{page.Title}' cannot be found in {_leftMenuFlatItems}.", page.PageId, page.Title, nameof(_leftMenuFlatItems));
            return Task.FromResult<IEnumerable<BreadcrumbItem>>(res);
        }

        if (find.Url == _appSettings.HomePage.PageId)
            return Task.FromResult<IEnumerable<BreadcrumbItem>>(res);

        BreadcrumbsRek(find, res);

        res.Last().Disabled = true;

        return Task.FromResult<IEnumerable<BreadcrumbItem>>(res);
    }

    private void ReBuildLeftMenu()
    {
        foreach (var item in _appSettings.LeftMenuHierarchy)
        {
            item.Parent = null;
            _leftMenuFlatItems.Add(item);
            ReBuildLeftMenuRecurrence(item.Children, item);
        }
    }

    private void ReBuildLeftMenuRecurrence(IEnumerable<MenuHierarchyItem> items, MenuHierarchyItem parent)
    {
        foreach (var item in items)
        {
            item.Parent = parent;
            _leftMenuFlatItems.Add(item);
            ReBuildLeftMenuRecurrence(item.Children, item);
        }
    }

    private void BreadcrumbsRek(MenuHierarchyItem item, ICollection<BreadcrumbItem> list)
    {
        if (item.Parent != null)
            BreadcrumbsRek(item.Parent, list);

        // if (item.PageId != null)
        list.Add(item.ToBreadcrumbItem());
    }

    private static MenuHierarchyItem? FindMenuItem(IEnumerable<MenuHierarchyItem> items, IPageData page)
    {
        return items.FirstOrDefault(i => i.Id == page.PageId);
    }
}