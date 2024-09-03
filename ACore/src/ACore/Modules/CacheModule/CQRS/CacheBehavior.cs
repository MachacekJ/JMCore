using ACore.CQRS;
using ACore.Models;
using MediatR;

namespace ACore.Modules.CacheModule.CQRS;

public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  /// <summary>
  /// Do not use for logging. Look at <see cref="CacheModuleRequest{TResponse}"/> and <see cref="LoggedRequest{TResponse}"/>.
  /// TODOo add telemetry
  /// </summary>
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    // var id = request.Id;
    var response = await next();
    return response;
  }
}