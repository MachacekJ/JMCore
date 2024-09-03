using ACore.CQRS;
using ACore.Models;

namespace ACore.Server.Modules.SettingModule.CQRS;

public abstract class SettingModuleRequestHandler<TRequest> : ICQRSRequestHandler<TRequest>
  where TRequest : IResultRequest
{
  public abstract Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
}

public abstract class SettingModuleRequestHandler<TRequest, TResponse> : ICQRSRequestHandler<TRequest, TResponse>
  where TRequest : IResultRequest<TResponse>
{
  public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}