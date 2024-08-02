using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.Models;
using ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid;
using ACore.AppTest.Modules.TestModule.CQRS.TestPKString;
using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Models;
using FluentAssertions;
using MediatR;
using Xunit;

namespace ACore.Tests.Server.Modules.TestModule;

/// <summary>
/// Two kinds of table primary key are supported. 
/// </summary>
// ReSharper disable once InconsistentNaming
public class AuditPKT : AuditAttributeBaseTests
{
  [Fact]
  public async Task GuidPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.GuidPK(AuditStorageModule, Mediator, GetTableName); });
  }

  [Fact]
  public async Task StringPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.StringPK(AuditStorageModule, Mediator, GetTableName); });
  }
}

// ReSharper disable once InconsistentNaming
public static class AuditPKTHelper
{
  private const string TestName = "AuditPK";
  private const string TestPKGuidEntityName = "TestPKGuidEntity";
    private const string TestPKStringEntityName = "TestPKStringEntity";
  public static async Task GuidPK(IAuditStorageModule auditStorageModule, IMediator mediator, Func<string, string> getTableName)
  {
    var id = Guid.NewGuid();
    
    // Arrange.
    var item = new TestPKGuidData()
    {
      Id = id,
      Name = TestName,
    };

    // Action 1
    var res = await mediator.Send(new TestPKGuidSaveCommand(item)); 
    res.Should().Be(true);

    // Assert 1
    var allData = await mediator.Send(new TestPKGuidGetQuery());
    allData.Should().HaveCount(1);
    
    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKGuidEntityName), item.Id.ToString());
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    auditVwAuditEntities.Count().Should().Be(2);
    auditVwAuditEntities.Single(a => a.NewValueGuid == item.Id).NewValueGuid.Should().Be(item.Id);

    // Arrange 2
    item.Name = TestName + "2";

    // Action 2 
    var res2 = await mediator.Send(new TestPKGuidSaveCommand(item)); 
    res2.Should().Be(true);


    // Assert 2
    auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKGuidEntityName), item.Id.ToString());
    auditValues.Count(a => a.PKValueString == item.Id.ToString()).Should().Be(3);
  }

  public static async Task StringPK(IAuditStorageModule auditStorageModule, IMediator mediator, Func<string, string> getTableName)
  {
    var id = Guid.NewGuid() + "ř Ř ě";

    // Arrange.
    var item = new TestPKStringData()
    {
      Id = id,
      Name = TestName,
    };

    // Action 1
    var res = await mediator.Send(new TestPKStringSaveCommand(item)); 
    res.Should().Be(true);

    // Assert 1
    var allData = await mediator.Send(new TestPKStringGetQuery());
    allData.Should().HaveCount(1);
    
    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKStringEntityName), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    auditVwAuditEntities.Count().Should().Be(2);
    auditVwAuditEntities.Single(a => a.NewValueString == item.Id).NewValueString.Should().Be(item.Id);

    // Arrange 2
    item.Name = TestName + "2";

    // Action 2 
    var res2 = await mediator.Send(new TestPKStringSaveCommand(item)); 
    res2.Should().Be(true);

    // Assert 2
    auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKStringEntityName), item.Id);
    auditValues.Count(a => a.PKValueString == item.Id).Should().Be(3);
  }
}