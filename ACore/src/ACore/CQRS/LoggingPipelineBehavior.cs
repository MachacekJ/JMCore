using System.Diagnostics;
using System.Text.Json;
using ACore.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.CQRS;

public class LoggingPipelineBehavior<TRequest, TResponse>(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var id = Guid.NewGuid();
    var duration = new Stopwatch();
    
    // Serialization could be expensive.
    if (logger.IsEnabled(LogLevel.Debug))
    {
      logger.LogDebug("Request '{request}'.Id:{id};Data:{data}", typeof(TRequest).Name, id, JsonSerializer.Serialize(request));
      duration.Start();
    }

    var response = await next();
    
    // Serialization could be expensive.
    if (logger.IsEnabled(LogLevel.Debug))
    {
      duration.Stop();
      logger.LogDebug("Response '{request}'.Id:{id};Duration:{duration};Data:{data}", typeof(TRequest).Name, id, duration, JsonSerializer.Serialize(response));
    }
    
    return response;
  }
}