using ACore.Base.CQRS.Results;
using MediatR;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public abstract class SettingsDbModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}