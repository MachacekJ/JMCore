using ACore.Models;
using MediatR;

namespace ACore.Server.Modules.SettingModule.CQRS;

public class SettingModulePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
   where TRequest : SettingModuleRequest<TResponse>
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