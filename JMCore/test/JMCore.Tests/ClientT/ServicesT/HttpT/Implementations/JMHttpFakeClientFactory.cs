using System.Net.Http.Headers;
using JMCore.Client.Services.Http;
using Moq;
using Moq.Protected;

namespace JMCore.Tests.ClientT.ServicesT.HttpT.Implementations;

public class JMHttpFakeClientFactory(List<FakeHttpClientConfiguration> responses) : IJMHttpClientFactory
{
    public Task<HttpClient> CreateAuthClientAsync()
    {
        var http = new HttpClient(MockedHttpResponse().Object);
        return Task.FromResult(http);
    }

    public Task<HttpClient> CreateNonAuthClientAsync()
    {
        var http = new HttpClient(MockedHttpResponse().Object);
        return Task.FromResult(http);
    }

    /// <summary>
    /// Mocks string response context for http.
    /// </summary>
    private Mock<HttpMessageHandler> MockedHttpResponse()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage httpRequestMessage, CancellationToken _) =>
            {
                if (httpRequestMessage.RequestUri == null)
                    throw new ArgumentException($"{nameof(HttpRequestMessage.RequestUri)} is null.");

                var url = (string.IsNullOrEmpty(httpRequestMessage.RequestUri.Query)
                    ? httpRequestMessage.RequestUri.AbsoluteUri
                    : httpRequestMessage.RequestUri.AbsoluteUri.Replace(httpRequestMessage.RequestUri.Query, string.Empty));
                    
                var response = responses.FirstOrDefault(r => r.CallingUrl == url);
                if (response == null)
                    throw new ArgumentException($"No response has been found for {httpRequestMessage.RequestUri.AbsoluteUri}.");

                var mockResponse = new HttpResponseMessage
                {
                    Content = new StringContent(response.ResponseJson),
                    StatusCode = response.StatusCode
                };
                mockResponse.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");

                response.SendActionCallback?.Invoke(httpRequestMessage, mockResponse);

                return mockResponse;
            });

        return mockHandler;
    }
}