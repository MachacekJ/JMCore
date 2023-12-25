namespace JMCore.Blazor.Services.Page.Interfaces;

public interface IPageData
{
    string Title { get; set; }
    string PageId { get; }
    public string PageUrl { get; }
    
    (Type Type, string Name)? ResX { get; }
}

