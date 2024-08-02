using ACore.Blazor.Services.Page.Interfaces;

namespace ACore.Blazor.Services.Page.Implementations;

public class PageData(string pageId, string title, (Type Type, string Name)? resX)
    : IPageData
{
    public string Title { get; set; } = title;
    public string PageId { get; } = pageId;
    public string PageUrl { get; set; } = pageId;
    public (Type Type, string Name)? ResX { get; } = resX;

    public PageData(string pageId, string pageUrl, string title, (Type Type, string Name)? resX) : this(pageId, title, resX)
    {
        PageUrl = pageUrl;
    }
}