using ACore.Blazor.Services.Page.Models;

namespace ACore.Blazor.Services.Page.Interfaces;

public interface IPageManager
{
    Task<IEnumerable<BreadcrumbItem>> GetBreadcrumbsForPageAsync(IPageData page);
}