using System.Diagnostics;
using System.Text.Json;
using ACore.Extensions;
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
    logger.LogInformation("Request '{request}'.Id:{id};Data:{data}", typeof(TRequest).Name, id, JsonSerializer.Serialize(request));
    duration.Start();
    var response = await next();
    duration.Stop();
    logger.LogInformation("Response '{request}'.Id:{id};Duration:{duration};Data:{data}", typeof(TRequest).Name, id, duration, JsonSerializer.Serialize(response));
    return response;
  }

  private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
  {
    while (toCheck != null && toCheck != typeof(object))
    {
      var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
      if (generic == cur)
      {
        return true;
      }

      toCheck = toCheck.BaseType;
    }

    return false;
  }
}