using ACore.Models;
using MediatR;

namespace ACore.Modules.MemoryCacheModule.CQRS;

public abstract class MemoryCacheModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}