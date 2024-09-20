using ACore.Base.CQRS.Models.Results;
using MediatR;

namespace ACore.Base.CQRS.Models;

public class HashEntityQueryRequest<TResponse>(bool withEntityHash): IRequest<TResponse>
  where TResponse : Result
{
  public bool WithEntityHash => withEntityHash;
}

public class HashEntityCommandRequest<TResponse>(string? hashToCheck): IRequest<TResponse>
  where TResponse : Result
{
  public string? Hash => hashToCheck;
}
