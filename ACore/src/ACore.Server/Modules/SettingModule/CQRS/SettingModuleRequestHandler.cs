using ACore.Models;
using MediatR;

namespace ACore.Server.Modules.SettingModule.CQRS;

public abstract class SettingModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}