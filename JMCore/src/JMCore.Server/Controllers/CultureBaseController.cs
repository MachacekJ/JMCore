using System.Threading.Tasks;
using JMCore.Models.BaseRR;
using JMCore.Server.Models.BaseRR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace JMCore.Server.Controllers;

public abstract class CultureBaseController<T> : BaseController<T> where T : BaseController<T>
{
    public async Task<ApiResponseBase<ApiResponseBaseDto>> SetCulture(HttpContext httpContext, string culture)
    {
        var res = new ApiResponseBase<ApiResponseBaseDto>();

        if (culture == null)
            return res;

        await RunInCatch(res, async () =>
        {
            CultureBase.SetCulture(httpContext, culture);
            await Task.CompletedTask;
        });

        return res;
    }
    
    public async Task<ApiResponseBase<ApiResponseBaseDto>> ResetCulture(HttpContext httpContext)
    {
        var res = new ApiResponseBase<ApiResponseBaseDto>();
        await RunInCatch(res, async () =>
        {
            httpContext.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            await Task.CompletedTask;
        });
        return res;
    }
}

public static class CultureBase
{
    public static void SetCulture(HttpContext httpContext, string culture)
    {
        httpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)));
    }
}