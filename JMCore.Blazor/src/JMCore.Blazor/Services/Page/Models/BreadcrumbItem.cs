using Telerik.SvgIcons;

namespace JMCore.Blazor.Services.Page.Models;

public class BreadcrumbItem
{
    public string Text { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public ISvgIcon? Icon { get; set; }
    public bool Disabled { get; set; }
    public string Url { get; set; } = string.Empty;
}
