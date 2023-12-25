using System.Security.Claims;
using JMCoreTest.Blazor.Server.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JMCoreTest.Blazor.Server.Controllers;

// orig src https://github.com/berhir/BlazorWebAssemblyCookieAuth
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly E2EConfiguration _e2EConfiguration;

    public AccountController(IOptions<E2EConfiguration> e2EOption)
    {
        _e2EConfiguration = e2EOption.Value;
    }

    [HttpGet("Login")]
    public ActionResult Login(string returnUrl)
    {
        if (!_e2EConfiguration.Enabled)
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
            });

        var cl = new Claim(ClaimTypes.Name, "test");
        var ls = new List<Claim>() { cl };
        var lss = new ClaimsIdentity(ls, CookieAuthenticationDefaults.AuthenticationScheme );
        var bb = new AuthenticationProperties
        {
            RedirectUri = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
        };

        return SignIn(new ClaimsPrincipal(lss), bb, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [ValidateAntiForgeryToken]
    [Authorize]
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        if (!_e2EConfiguration.Enabled)
        {
            return SignOut(new AuthenticationProperties
                {
                    RedirectUri = "/"
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/"
            },
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }
}