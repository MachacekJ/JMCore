namespace JMCore.Client.Services.Http;

public interface IJMHttpClientFactory
{
    Task<HttpClient> CreateAuthClientAsync();
    Task<HttpClient> CreateNonAuthClientAsync();
}
