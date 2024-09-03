using ACore.CQRS;

namespace ACore.Server.Modules.AuditModule.CQRS;

public class AuditModuleRequest<TResponse> : IResultRequest<TResponse>;
public class AuditModuleRequest : IResultRequest;