using ACore.CQRS;

namespace ACore.Server.Modules.AuditModule.CQRS;

public class AuditModuleRequest<TResponse> : LoggedRequest<TResponse>;