using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ACore.CQRS;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : LoggedRequest<TResponse>
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var id = request.Id;
    var duration = new Stopwatch();
    logger.LogInformation("Request '{request}'.Id:{id};Data:{data}", typeof(TRequest).Name, id, JsonSerializer.Serialize(request));
    duration.Start();
    var response = await next();
    duration.Stop();
    logger.LogInformation("Response '{request}'.Id:{id};Duration:{duration};Data:{data}", typeof(TRequest).Name, id, duration, JsonSerializer.Serialize(response));
    return response;
  }
}