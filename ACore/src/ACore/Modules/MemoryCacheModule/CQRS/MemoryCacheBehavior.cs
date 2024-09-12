using ACore.Base;
using ACore.Configuration;
using ACore.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Modules.MemoryCacheModule.CQRS;

public class MemoryCacheBehavior<TRequest, TResponse>(IOptions<ACoreOptions> coreOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new BehaviorHelper<TResponse>();
    
    if (!moduleBehaviorHelper.CheckIfModuleIsActive(coreOptions.Value.MemoryCacheModuleOptions, nameof(ACoreExtension.AddACore) , out var resultError))
      return resultError ?? throw new Exception($"{nameof(BehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");

    var response = await next();
    return response;
  }
}