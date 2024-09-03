using ACore.CQRS;

namespace ACore.Modules.CacheModule.CQRS;

public class CacheModuleRequest<TResponse> : IResultRequest<TResponse>;