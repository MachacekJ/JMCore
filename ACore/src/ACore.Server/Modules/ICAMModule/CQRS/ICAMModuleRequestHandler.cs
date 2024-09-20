using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using MediatR;

namespace ACore.Server.Modules.ICAMModule.CQRS;

public abstract class ICAMModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}