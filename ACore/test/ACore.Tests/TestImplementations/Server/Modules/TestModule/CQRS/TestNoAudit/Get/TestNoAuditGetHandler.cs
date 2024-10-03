using ACore.Base.CQRS.Models.Results;
using ACore.Server.Configuration.CQRS.OptionsGet;
using ACore.Server.Storages;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Get;

internal class TestNoAuditGetHandler(IStorageResolver storageResolver, IMediator mediator) : TestModuleRequestHandler<TestNoAuditGetQuery, Result<Dictionary<string, TestNoAuditData>>>(storageResolver)
{
  public override async Task<Result<Dictionary<string, TestNoAuditData>>> Handle(TestNoAuditGetQuery request, CancellationToken cancellationToken)
  {
    var saltForHash = (await mediator.Send(new AppOptionQuery<string>(OptionQueryEnum.HashSalt), cancellationToken)).ResultValue 
                      ?? throw new Exception($"Mediator for {nameof(AppOptionQuery<string>)}.{Enum.GetName(OptionQueryEnum.HashSalt)} returned null value.");
    
    var db = ReadTestContext().DbSet<TestNoAuditEntity, int>() ?? throw new Exception();
    var testData = new Dictionary<string, TestNoAuditData>(await db.Select(a => TestNoAuditData.Create(a, saltForHash)).ToArrayAsync(cancellationToken: cancellationToken));
    return Result.Success(testData);
  }
}