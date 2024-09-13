using MediatR;

namespace ACore.Server.Modules.AuditModule.CQRS;

public class AuditModuleRequest<TResponse> : IRequest<TResponse>;