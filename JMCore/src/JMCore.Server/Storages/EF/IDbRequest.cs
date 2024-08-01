using MediatR;

namespace JMCore.Server.Storages.EF
{
    public interface IDbRequest : IRequest;
    public interface IDbRequest<out TUnit> : IRequest<TUnit>;
}
