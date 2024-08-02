using ACore.CQRS;
using MediatR;


namespace ACore.Tests.Implementations.Modules.TestModule.CQRS;

public class TestModuleBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : TestModuleRequest<TResponse>
{
  /// <summary>
  /// Do not use for logging. Look at <see cref="TestModuleRequest{TResponse}"/> and <see cref="LoggedRequest{TResponse}"/>.
  /// TODOo add telemetry
  /// </summary>
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    var response = await next();
    return response;
  }
}