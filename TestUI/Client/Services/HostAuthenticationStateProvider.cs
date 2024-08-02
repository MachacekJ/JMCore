using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using ACore.Client.Services.Http;
using JMCoreTest.Blazor.Shared.Authorization;

namespace JMCoreTest.Blazor.Client.Services;

// orig src https://github.com/berhir/BlazorWebAssemblyCookieAuth
public class HostAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly TimeSpan UserCacheRefreshInterval = TimeSpan.FromSeconds(60);

    private const string LogInPath = "api/Account/Login";
    //private const string LogOutPath = "api/Account/Logout";

    private readonly NavigationManager _navigation;

    private readonly ILogger<HostAuthenticationStateProvider> _logger;
    private readonly IJMHttpClientFactory _httpClientFactory;

    private HttpClient? _clientMemory;
    private DateTimeOffset _userLastCheck = DateTimeOffset.FromUnixTimeSeconds(0);
    private ClaimsPrincipal _cachedUser = new(new ClaimsIdentity());

    public HostAuthenticationStateProvider(NavigationManager navigation, IJMHttpClientFactory httpClientFactory, ILogger<HostAuthenticationStateProvider> logger)
    {
        _navigation = navigation;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() => new(await GetUser(useCache: true));

    public void SignIn(string? customReturnUrl = null)
    {
        var returnUrl = customReturnUrl != null ? _navigation.ToAbsoluteUri(customReturnUrl).ToString() : null;
        var encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? _navigation.Uri);
        var logInUrl = _navigation.ToAbsoluteUri($"{LogInPath}?returnUrl={encodedReturnUrl}");
        _navigation.NavigateTo(logInUrl.ToString(), true);
    }

    private async ValueTask<ClaimsPrincipal> GetUser(bool useCache = false)
    {
        var now = DateTimeOffset.Now;
        if (useCache && now < _userLastCheck + UserCacheRefreshInterval)
        {
            _logger.LogDebug("Taking user from cache");
            return _cachedUser;
        }

        _logger.LogDebug("Fetching user");
        _cachedUser = await FetchUser();
        _userLastCheck = now;

        return _cachedUser;
    }

    private async Task<ClaimsPrincipal> FetchUser()
    {
        UserInfo? user = null;
        var httpClient = await GetClientAsync();
        try
        {
            _logger.LogInformation("{clientBaseAddress}", httpClient.BaseAddress?.ToString());
            user = await httpClient.GetFromJsonAsync<UserInfo>("api/User");
        }
        catch (Exception exc)
        {
            _logger.LogWarning(exc, "Fetching user failed.");
        }

        if (user == null || !user.IsAuthenticated)
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        var identity = new ClaimsIdentity(
            nameof(HostAuthenticationStateProvider),
            user.NameClaimType,
            user.RoleClaimType);

        identity.AddClaims(user.Claims.Select(c => new Claim(c.Type, c.Value)));
        return new ClaimsPrincipal(identity);
    }

    private async Task<HttpClient> GetClientAsync()
        => _clientMemory ??= await _httpClientFactory.CreateNonAuthClientAsync();
}