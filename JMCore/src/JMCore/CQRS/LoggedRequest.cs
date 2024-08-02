using MediatR;

namespace JMCore.CQRS;

public class LoggedRequest<TResponse> : IRequest<TResponse>
{
  public Guid Id { get; } = Guid.NewGuid();
}