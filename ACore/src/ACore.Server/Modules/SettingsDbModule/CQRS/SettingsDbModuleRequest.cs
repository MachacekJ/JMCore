using ACore.Base.CQRS.Results;
using MediatR;

namespace ACore.Server.Modules.SettingsDbModule.CQRS;

public class SettingsDbModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;