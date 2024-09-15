using ACore.Base.CQRS.Helpers;
using ACore.Base.CQRS.Models;
using ACore.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Modules.MemoryCacheModule.CQRS;

public class MemoryCacheModulePipelineBehavior<TRequest, TResponse>(IOptions<ACoreOptions> coreOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();
    
    if (!moduleBehaviorHelper.CheckIfModuleIsActive(coreOptions.Value.MemoryCacheModuleOptions, nameof(ACoreServiceExtension.AddACore) , out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");

    var response = await next();
    return response;
  }
}