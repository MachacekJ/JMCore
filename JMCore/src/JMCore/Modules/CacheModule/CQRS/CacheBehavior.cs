using JMCore.CQRS;
using MediatR;

namespace JMCore.Modules.CacheModule.CQRS;

public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : CacheModuleRequest<TResponse>
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