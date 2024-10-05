using ACore.Base.CQRS.Pipelines.Helpers;
using ACore.Base.CQRS.Results;
using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.CQRS;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS;

public class AuditModulePipelineBehavior<TRequest, TResponse>(IOptions<ACoreServerOptions> serverOptions) : IPipelineBehavior<TRequest, TResponse>
   where TRequest : SettingsDbModuleRequest<TResponse>
   where TResponse : Result
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();

    if (!moduleBehaviorHelper.CheckIfModuleIsActive(serverOptions.Value.AuditModuleOptions, nameof(ACoreServerServiceExtensions.AddACoreServer), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");

    return await next();
  }
}