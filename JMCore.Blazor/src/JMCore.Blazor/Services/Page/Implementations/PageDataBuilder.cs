using JMCore.ResX;

namespace JMCore.Blazor.Services.Page.Implementations;

public class PageDataBuilder
{
    private readonly string _pageId;
    private readonly string _title;
    public const string PageNotFoundId = "notFound";
    private string _pageUrl;
    private (Type Type, string Name)? _resx;

    public PageDataBuilder(string pageId, string title)
    {
        _pageId = pageId.ToLower();
        if (_pageId.StartsWith("/"))
            _pageId = _pageId.Substring(1);
        _pageUrl = _pageId;
        _title = title;
    }

    public PageDataBuilder SetResX(Type type, string name)
    {
        _resx = new ValueTuple<Type, string>(type, name);
        return this;
    }

    public PageDataBuilder SetUrl(string url1)
    {
        _pageUrl = url1;
        return this;
    }

    public PageData Build()
    {
        return new PageData(_pageId, _pageUrl, _title, _resx);
    }

    public static PageData BuildNotFoundPage()
    {
        return new PageData(PageNotFoundId, "Page not found", (typeof(ResX_General), ResX_General.PageNotFound));
    }
}