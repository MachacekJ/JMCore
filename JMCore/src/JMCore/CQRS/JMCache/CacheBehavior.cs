using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JMCore.CQRS.JMCache;

public class CacheBehavior<TRequest, TResponse>(ILogger<CacheBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ICacheRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Cache request: {typeof(TRequest).Name}");
        var response = await next();
        logger.LogInformation($"Cache response: { JsonSerializer.Serialize(response)}");
        return response;
    }
}