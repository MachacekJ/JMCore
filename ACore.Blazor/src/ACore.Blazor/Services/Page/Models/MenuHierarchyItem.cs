using ACore.Blazor.Services.Page.Interfaces;
using Telerik.Blazor.Components.Common.Trees.Models;
using Telerik.Blazor.Components.PanelBar.Models;
using Telerik.SvgIcons;

namespace ACore.Blazor.Services.Page.Models;

public class MenuHierarchyItem
{
    public (Type Type, string Name)? ResX { get; }
    //public string? PageId { get; }

    public string Id { get; }

    public string Title { get; set; }
    public string? Url { get; }
    public ISvgIcon? Icon { get; }

    public IEnumerable<MenuHierarchyItem> Children { get; } = Array.Empty<MenuHierarchyItem>();
    public MenuHierarchyItem? Parent { get; set; }

    public MenuHierarchyItem(IPageData page, ISvgIcon? icon = null)
    {
        Id = page.PageId;
        Title = page.Title;
        ResX = page.ResX;
        var url = "/";

        if (!string.IsNullOrEmpty(page.PageUrl))
            if (!page.PageUrl.StartsWith("/"))
                url += page.PageUrl;

        Url = url;
        Icon = icon;
    }
    public MenuHierarchyItem(string id, string title, ISvgIcon icon, IEnumerable<MenuHierarchyItem> children) :this(id, title, null, icon, children)
    {
     
    }
    public MenuHierarchyItem(string id, string title, (Type Type, string Name)? resx, ISvgIcon icon, IEnumerable<MenuHierarchyItem> children)
    {
        Id = id;
        ResX = resx;
        Title = title;
        Icon = icon;
        Children = children;
    }
    
    public MenuHierarchyItem(IPageData page, ISvgIcon icon, IEnumerable<MenuHierarchyItem> children):this(page, icon)
    {
        Children = children;
    }

}

public static class MenuHierarchyItemExtension
{
    public static PanelBarItem ToPanelBarItem(this MenuHierarchyItem menuHierarchyItem)
    {
        return new PanelBarItem()
        {
            Id = menuHierarchyItem.Id,
            Items = new List<TreeItem>(),
            Text = menuHierarchyItem.Title,
            Icon = menuHierarchyItem.Icon,
            Url = menuHierarchyItem.Url
        };
    }

    public static BreadcrumbItem ToBreadcrumbItem(this MenuHierarchyItem menuHierarchyItem)
    {
        return new BreadcrumbItem()
        {
            Text = menuHierarchyItem.Title,
            Title = menuHierarchyItem.Title,
            Icon = menuHierarchyItem.Icon,
            Url = menuHierarchyItem.Url ?? string.Empty
        };
    }
}