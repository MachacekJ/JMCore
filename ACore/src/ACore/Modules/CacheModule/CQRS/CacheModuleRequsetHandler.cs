using ACore.Models;
using MediatR;

namespace ACore.Modules.CacheModule.CQRS;

public abstract class CacheModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}