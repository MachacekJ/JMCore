using JMCore.CQRS;

namespace JMCore.Modules.CacheModule.CQRS;

public class CacheModuleRequest<TResponse> : LoggedRequest<TResponse>;