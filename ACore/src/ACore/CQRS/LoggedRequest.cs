using MediatR;

namespace ACore.CQRS;

public class LoggedRequest<TResponse> : IRequest<TResponse>
{
  public Guid Id { get; } = Guid.NewGuid();
}