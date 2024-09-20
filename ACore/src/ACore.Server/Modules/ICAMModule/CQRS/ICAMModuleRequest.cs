using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using MediatR;

namespace ACore.Server.Modules.ICAMModule.CQRS;

public class ICAMModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;