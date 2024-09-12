﻿using System.Diagnostics;
using System.Text.Json;
using ACore.Base;
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
    TResponse? response;

    // Serialization could be expensive for all requests.
    if (logger.IsEnabled(LogLevel.Debug))
    {
      logger.LogDebug("Request '{request}'.Id:{id};Data:{data}", typeof(TRequest).Name, id, JsonSerializer.Serialize(request));
      duration.Start();
    }

    try
    {
      response = await next();
    }
    catch (Exception e)
    {
      response = BehaviorHelper<TResponse>.CreateErrorExceptionResult<TResponse>(e);
    }

    // Analyzes errors for logging
    if (response.IsFailure)
    {
      // It is only validation error, it may not be logged.
      var isSeriousError = !(response is ValidationResult || response.GetType().GetGenericTypeDefinition() == typeof(ValidationResult<>));

      // This is a serious exception error.
      if (isSeriousError && (response is ExceptionResult || response.GetType().GetGenericTypeDefinition() == typeof(ExceptionResult<>)))
      {
        var aa = (Exception)(response.PropertyValue(nameof(ExceptionResult.Exception)) ?? throw new Exception($"{nameof(ExceptionResult.Exception)} doesn't exist."));
        LogError(request, response, aa.MessageRecur(true));
        isSeriousError = false;
      }

      // This is s serious error e.g. configuration.
      if (isSeriousError)
        LogError(request, response);
      
    }

    if (!logger.IsEnabled(LogLevel.Debug))
      return response;

    duration.Stop();
    logger.LogDebug("Response '{request}'.Id:{id};Duration:{duration};Data:{data}", typeof(TRequest).Name, id, duration, JsonSerializer.Serialize(response));

    return response;
  }

  private void LogError(TRequest request, TResponse response, string exception = "")
  {
    var dataRequest = string.Empty;
    var dataResponse = string.Empty;

    try
    {
      dataRequest = JsonSerializer.Serialize(request);
      dataResponse = JsonSerializer.Serialize(response);
    }
    catch (Exception e)
    {
      exception += e.MessageRecur(true);
    }

    logger.LogError("ErrorId:'{errorId}'; Request:'{requestName}'; ErrorCode:{errorCode}; Error{error}; DataRequest{dataRequest}; DataResponse:{dataResponse}",
      response.Id, typeof(TRequest).Name, response.Error.Code, response.Error.Message + exception, dataRequest, dataResponse);
  }
}