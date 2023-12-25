using JMCore.Client.Services.Http;
using Microsoft.JSInterop;

namespace JMCoreTest.Blazor.Client.Services;

public class AntiforgeryHttpClientFactory : IJMHttpClientFactory
{
    public const string NonAuthorizedClientName = "default";
    public const string AuthorizedClientName = "authorizedClient";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJSRuntime _jSRuntime;

    public AntiforgeryHttpClientFactory(IHttpClientFactory httpClientFactory, IJSRuntime jSRuntime)
    {
        _httpClientFactory = httpClientFactory;
        _jSRuntime = jSRuntime;
    }

    public async Task<HttpClient> CreateAuthClientAsync()
    {
        return await CreateClientAsync(AuthorizedClientName);
    }

    public async Task<HttpClient> CreateNonAuthClientAsync()
    {
        return await CreateClientAsync(NonAuthorizedClientName);
    }
    private async Task<HttpClient> CreateClientAsync(string clientName)
    {
        var token = await _jSRuntime.InvokeAsync<string>("getAntiForgeryToken");

        var client = _httpClientFactory.CreateClient(clientName);
        client.DefaultRequestHeaders.Add("X-XSRF-TOKEN", token);

        return client;
    }    
    
}
