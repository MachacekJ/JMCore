using MediatR;

namespace JMCore.Server.Storages.Base.EF
{
    public interface IDbRequest : IRequest;
    public interface IDbRequest<out TUnit> : IRequest<TUnit>;
}
