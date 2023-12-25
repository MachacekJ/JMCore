using MediatR;

namespace JMCore.Server.DB.Abstract
{
    public interface IDbRequest : IRequest;
    public interface IDbRequest<out TUnit> : IRequest<TUnit>;
}
