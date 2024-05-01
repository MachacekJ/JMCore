using MediatR;

namespace JMCore.Server.Storages.Abstract
{
    public interface IDbRequest : IRequest;
    public interface IDbRequest<out TUnit> : IRequest<TUnit>;
}
