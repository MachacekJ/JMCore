using ACore.Base.CQRS.Models;
using ACore.Models;
using MediatR;

namespace ACore.Server.Modules.SettingModule.CQRS;

public class SettingModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;