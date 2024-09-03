using ACore.Models;

namespace ACore.CQRS;

public interface ICQRSRequestHandler<in TRequest> : MediatR.IRequestHandler<TRequest, Result>
  where TRequest : IResultRequest;

public interface ICQRSRequestHandler<in TRequest, TResponse>
  : MediatR.IRequestHandler<TRequest, Result<TResponse>>
  where TRequest : IResultRequest<TResponse>;