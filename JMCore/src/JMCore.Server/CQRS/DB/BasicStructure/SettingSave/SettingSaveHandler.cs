using JMCore.Server.Storages.DbContexts.BasicStructure;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingSave;

public class SettingSaveHandler : BasicDbRequestHandler<SettingSaveCommand>
{
    public SettingSaveHandler(IBasicDbContext basicDbContext) : base(basicDbContext)
    {
    }

    public override async Task Handle(SettingSaveCommand request, CancellationToken cancellationToken)
    {
        await BasicDbContext.Setting_SaveAsync(request.Key, request.Value, request.IsSystem);
    }
}