using ACore.Base.CQRS.Helpers;
using ACore.Base.CQRS.Models;
using ACore.Server.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public class SettingsDbModulePipelineBehavior<TRequest, TResponse>(IOptions<ACoreServerOptions> serverOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : SettingsDbModuleRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();

    if (!moduleBehaviorHelper.CheckIfModuleIsActive(serverOptions.Value.SettingsDbModuleOptions, nameof(ACoreServerServiceExtensions.AddACoreServer), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");

    return await next();
  }
}