using ACore.CQRS;
using ACore.Models;

namespace ACore.Modules.CacheModule.CQRS;

public abstract class CacheModuleRequestHandler<TRequest> : ICQRSRequestHandler<TRequest>
  where TRequest : IResultRequest
{
  public abstract Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
}

public abstract class CacheModuleRequestHandler<TRequest, TResponse> : ICQRSRequestHandler<TRequest, TResponse>
  where TRequest : IResultRequest<TResponse>
{
  public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}