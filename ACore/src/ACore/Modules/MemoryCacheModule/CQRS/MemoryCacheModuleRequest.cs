using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Models;
using MediatR;

namespace ACore.Modules.MemoryCacheModule.CQRS;

public class MemoryCacheModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;