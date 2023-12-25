using JMCore.Server.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JMCoreTest.Blazor.Server.Controllers;

[ValidateAntiForgeryToken]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class DirectApiController : BaseController<DirectApiController>
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        var res = new List<string> { "some data", "more data", "loads of data" };
        return res;
    }

    public DirectApiController(ILogger<DirectApiController> logger) : base(logger)
    {
    }
}
