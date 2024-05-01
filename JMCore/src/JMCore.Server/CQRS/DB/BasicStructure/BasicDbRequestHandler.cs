using JMCore.Server.Storages.DbContexts.BasicStructure;
using MediatR;

namespace JMCore.Server.CQRS.DB.BasicStructure
{
    public abstract class BasicDbRequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract Task Handle(TRequest request, CancellationToken cancellationToken);

        protected readonly IBasicDbContext BasicDbContext;

        protected BasicDbRequestHandler(IBasicDbContext basicDbContext)
        {
            BasicDbContext = basicDbContext;
        }
    }

    public abstract class BasicDbRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected readonly IBasicDbContext BasicDbContext;

        protected BasicDbRequestHandler(IBasicDbContext basicDbContext)
        {
            BasicDbContext = basicDbContext;
        }
    }
}
