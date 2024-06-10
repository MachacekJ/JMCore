using System.Reflection;
using FluentAssertions;
using JMCore.Server.Storages.Modules.AuditModule.Models;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

// ReSharper disable InconsistentNaming

namespace JMCore.Tests.ServerT.StoragesT.AuditStorageT;

public class AuditPKT : AuditAttributeBaseT
{
  const string testName = "AuditPK";
  private const string tableGuidName = nameof(TestPKGuidEntity);
  private const string tableStringName = nameof(TestPKStringEntity);

  [Fact]
  public async Task GuidPK()
  {
    var Id = Guid.NewGuid();

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange.
      var item = new TestPKGuidEntity()
      {
        Id = Id,
        Name = testName,
      };

      // Action 1
      await TestStorageEfContext.TestPKGuid.AddAsync(item);
      await TestStorageEfContext.SaveChangesAsync();

      // Assert 1
      Assert.Equal(1, await TestStorageEfContext.TestPKGuid.CountAsync());
      var auditValues = await AuditEfStorageEfContext.AuditItemsAsync(tableGuidName, item.Id.ToString());
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      auditVwAuditEntities.Count().Should().Be(2);
      auditVwAuditEntities.Single(a => a.NewValueGuid == item.Id).NewValueGuid.Should().Be(item.Id);

      // Arrange 2
      item.Name = testName + "2";

      // Action 2 
      await TestStorageEfContext.SaveChangesAsync();

      // Assert 2
      auditValues = await AuditEfStorageEfContext.AuditItemsAsync(tableGuidName, item.Id.ToString());
      auditValues.Count(a => a.PKValueString == item.Id.ToString()).Should().Be(3);
    });
  }

  [Fact]
  public async Task StringPK()
  {
    var Id = Guid.NewGuid().ToString() + "ř Ř ě";

    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange.
      var item = new TestPKStringEntity()
      {
        Id = Id,
        Name = testName,
      };

      // Action 1
      await TestStorageEfContext.TestPKString.AddAsync(item);
      await TestStorageEfContext.SaveChangesAsync();

      // Assert 1
      Assert.True(await TestStorageEfContext.TestPKString.CountAsync() == 1);
      var auditValues = await AuditEfStorageEfContext.AuditItemsAsync(tableStringName, item.Id);
      var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
      auditVwAuditEntities.Count().Should().Be(2);
      auditVwAuditEntities.Single(a => a.NewValueString == item.Id).NewValueString.Should().Be(item.Id);

      // Arrange 2
      item.Name = testName + "2";

      // Action 2 
      await TestStorageEfContext.SaveChangesAsync();

      // Assert 2
      auditValues = await AuditEfStorageEfContext.AuditItemsAsync(tableStringName, item.Id);
      auditValues.Count(a => a.PKValueString == item.Id).Should().Be(3);
    });
  }
}