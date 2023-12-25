using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace JMCoreTest.Blazor.Client.Services;

public class AuthorizedHandler : DelegatingHandler
{
    private readonly HostAuthenticationStateProvider _authenticationStateProvider;
    private readonly IAccessTokenProviderAccessor _tokenProviderAccessor;

    public AuthorizedHandler(HostAuthenticationStateProvider authenticationStateProvider, IAccessTokenProviderAccessor tokenProviderAccessor)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _tokenProviderAccessor = tokenProviderAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // skip token endpoints
        if (request.RequestUri?.AbsolutePath.Contains("/tokens") is not true)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            if (authState.User.Identity != null && !authState.User.Identity.IsAuthenticated)
            {
                // if user is not authenticated, immediately set response status to 401 Unauthorized
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            if (await _tokenProviderAccessor.TokenProvider.GetAccessTokenAsync() is { } token)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                // if server returned 401 Unauthorized, redirect to login page
                _authenticationStateProvider.SignIn();
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

public static class AccessTokenProviderExtensions
{
    public static async Task<string?> GetAccessTokenAsync(this IAccessTokenProvider tokenProvider) =>
        (await tokenProvider.RequestAccessToken())
        .TryGetToken(out var token)
            ? token.Value
            : null;
}