using MediatR;

namespace JMCore.CQRS.JMCache;

public interface ICacheRequest<out TResponse> : IRequest<TResponse>;