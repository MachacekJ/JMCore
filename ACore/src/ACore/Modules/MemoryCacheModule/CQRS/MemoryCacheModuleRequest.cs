using ACore.Base.CQRS.Results;
using MediatR;

namespace ACore.Modules.MemoryCacheModule.CQRS;

public class MemoryCacheModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;