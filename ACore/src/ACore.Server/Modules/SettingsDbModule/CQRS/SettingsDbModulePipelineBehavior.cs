using ACore.Base.CQRS.Models;
using MediatR;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public class SettingsDbModulePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
   where TRequest : SettingsDbModuleRequest<TResponse>
   where TResponse : Result
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
      return await next();
  }
}