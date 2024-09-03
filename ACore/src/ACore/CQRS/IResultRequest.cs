using ACore.Models;

namespace ACore.CQRS;

public interface IResultRequest : MediatR.IRequest<Result>;

public interface IResultRequest<TResponse> : MediatR.IRequest<Result<TResponse>>;