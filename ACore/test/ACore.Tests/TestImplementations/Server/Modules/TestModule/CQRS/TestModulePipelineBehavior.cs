using ACore.Base.CQRS.Helpers;
using ACore.Base.CQRS.Models.Results;
using ACore.Tests.TestImplementations.Server.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS;

internal class TestModulePipelineBehavior<TRequest, TResponse>(IOptions<ACoreTestOptions> testOptions) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : TestModuleRequest<TResponse>
  where TResponse : Result
{
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var moduleBehaviorHelper = new PipelineBehaviorHelper<TResponse>();
    if (!moduleBehaviorHelper.CheckIfModuleIsActive(testOptions.Value.TestModuleOptions, nameof(ACoreTestServiceExtensions.AddACoreTest), out var resultError))
      return resultError ?? throw new Exception($"{nameof(PipelineBehaviorHelper<TResponse>.CheckIfModuleIsActive)} returned null result value.");
    
    return await next();
  }
}