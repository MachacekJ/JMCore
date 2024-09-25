using ACore.Base.CQRS.Models.Results;
using MediatR;

namespace ACore.Base.CQRS.Models;

public class EntityQueryRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;

public class EntityCommandRequest<TResponse>(string? hashToCheck): IRequest<TResponse>
  where TResponse : Result
{
  public string? Hash => hashToCheck;
}
