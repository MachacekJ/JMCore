using System.Reflection;
using FluentAssertions;
using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Modules.AuditModule.Storage.Models;
using JMCore.Tests.Implementations.Storages.TestModule.Models;
using JMCore.Tests.Implementations.Storages.TestModule.Storages;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;

/// <summary>
/// Two kinds of table primary key are supported. 
/// </summary>
// ReSharper disable once InconsistentNaming
public class AuditPKT : AuditAttributeBaseT
{
  [Fact]
  public async Task GuidPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.GuidPK(AuditStorageModule, TestStorageModule, (name) => name); });
  }

  [Fact]
  public async Task StringPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.StringPK(AuditStorageModule, TestStorageModule, (name) => name); });
  }
}

// ReSharper disable once InconsistentNaming
public static class AuditPKTHelper
{
  const string TestName = "AuditPK";
  public static async Task GuidPK(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<string, string> getTableName)
  {
    var id = Guid.NewGuid();

    // Arrange.
    var item = new TestPKGuidEntity()
    {
      Id = id,
      Name = TestName,
    };

    // Action 1
    await testStorageEfContext.AddAsync(item);

    // Assert 1
    (await testStorageEfContext.AllTestPKGuid()).Count().Should().Be(1);

    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(nameof(TestPKGuidEntity)), item.Id.ToString());
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    auditVwAuditEntities.Count().Should().Be(2);
    auditVwAuditEntities.Single(a => a.NewValueGuid == item.Id).NewValueGuid.Should().Be(item.Id);

    // Arrange 2
    item.Name = TestName + "2";

    // Action 2 
    await testStorageEfContext.UpdateAsync(item);

    // Assert 2
    auditValues = await auditStorageModule.AuditItemsAsync(getTableName(nameof(TestPKGuidEntity)), item.Id.ToString());
    auditValues.Count(a => a.PKValueString == item.Id.ToString()).Should().Be(3);
  }

  public static async Task StringPK(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, Func<string, string> getTableName)
  {
    var id = Guid.NewGuid() + "ř Ř ě";

    // Arrange.
    var item = new TestPKStringEntity()
    {
      Id = id,
      Name = TestName,
    };

    // Action 1
    await testStorageEfContext.AddAsync(item);

    // Assert 1
    (await testStorageEfContext.AllTestPKString()).Count().Should().Be(1);
    var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(nameof(TestPKStringEntity)), item.Id);
    var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    auditVwAuditEntities.Count().Should().Be(2);
    auditVwAuditEntities.Single(a => a.NewValueString == item.Id).NewValueString.Should().Be(item.Id);

    // Arrange 2
    item.Name = TestName + "2";

    // Action 2 
    await testStorageEfContext.UpdateAsync(item);

    // Assert 2
    auditValues = await auditStorageModule.AuditItemsAsync(getTableName(nameof(TestPKStringEntity)), item.Id);
    auditValues.Count(a => a.PKValueString == item.Id).Should().Be(3);
  }
}