using JMCore.Server.DB.DbContexts.BasicStructure;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingGet
{
    public class SettingGetHandler : BasicDbRequestHandler<SettingGetQuery, string?>
    {
        public SettingGetHandler(IBasicDbContext basicDbContext) : base(basicDbContext)
        {
        }
        
        public override Task<string?> Handle(SettingGetQuery request, CancellationToken cancellationToken)
        {
            return BasicDbContext.Setting_GetAsync(request.Key, request.IsRequired);
        }
    }
}
