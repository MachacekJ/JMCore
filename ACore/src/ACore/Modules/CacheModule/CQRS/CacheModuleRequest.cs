using ACore.Models;
using MediatR;

namespace ACore.Modules.CacheModule.CQRS;

public class CacheModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;