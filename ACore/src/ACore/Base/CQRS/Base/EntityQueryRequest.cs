using ACore.Base.CQRS.Results;
using MediatR;

namespace ACore.Base.CQRS.Base;

public class EntityQueryRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;

public class EntityCommandRequest<TResponse>(string? hashToCheck): IRequest<TResponse>
  where TResponse : Result
{
  public string? Hash => hashToCheck;
}
