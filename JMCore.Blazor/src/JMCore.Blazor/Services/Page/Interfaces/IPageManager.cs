using JMCore.Blazor.Services.Page.Interfaces;
using JMCore.Blazor.Services.Page.Models;

namespace JMCore.Blazor.Services.Page;

public interface IPageManager
{
    Task<IEnumerable<BreadcrumbItem>> GetBreadcrumbsForPageAsync(IPageData page);
}