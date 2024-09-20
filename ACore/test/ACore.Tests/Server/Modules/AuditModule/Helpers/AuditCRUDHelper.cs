using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Delete;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using FluentAssertions;
using MediatR;

namespace ACore.Tests.Server.Modules.AuditModule.Helpers;

public static class AuditCRUDHelper
{
  private static readonly DateTime TestDateTime = DateTime.UtcNow;
  private const string TestName = "AuditTest";

  public static async Task NoAuditAsyncTest(IMediator mediator, Func<string, string> getTableName)
  {
    const string entityName = nameof(TestNoAuditEntity);

    // Arrange
    var item = new TestNoAuditData(TestName)
    {
      Created = TestDateTime,
    };

    // Action
    var result = (await mediator.Send(new TestNoAuditSaveCommand(item))) as DbSaveResult;

    // Assert
    var allData = (await mediator.Send(new TestNoAuditGetQuery())).ResultValue;
    var itemId = AuditAssertHelpers.AssertSinglePrimaryKeyWithResult<TestNoAuditData, int>(result, allData);
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestNoAuditEntity, int>(getTableName(entityName), itemId))).ResultValue;

    resAuditItems.Should().HaveCount(0);
  }

  public static async Task AddItemAsyncTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    const string entityName = nameof(TestAuditEntity);

    // Arrange
    var item = new TestAuditData<int>
    {
      Created = TestDateTime,
      Name = TestName,
      NotAuditableColumn = "Audit"
    };

    // Action.
    var result = await mediator.Send(new TestAuditSaveCommand<int>(item)) as DbSaveResult;


    // Assert.
    var allData = (await mediator.Send(new TestAuditGetQuery<int>())).ResultValue;
    var itemId = AuditAssertHelpers.AssertSinglePrimaryKeyWithResult<TestAuditData<int>, int>(result, allData);
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestAuditEntity, int>(getTableName(entityName), itemId))).ResultValue;

    resAuditItems.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(1);
    resAuditItems.Single().EntityState.Should().Be(AuditStateEnum.Added);

    var auditItem = resAuditItems.Single();
    auditItem.Columns.Should().HaveCount(3);
    auditItem.Columns.All(c => c.IsChange).Should().Be(true);

    var aid = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditEntity.Created)));

    aid.Should().NotBeNull();
    aName.Should().NotBeNull();
    aName?.NewValue.Should().NotBeNull();
    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    aCreated.NewValue.Should().NotBeNull();

    aid?.NewValue.Should().Be(itemId);
    aName?.NewValue.Should().Be(TestName);
    new DateTime(Convert.ToInt64(aCreated.NewValue)).Should().Be(TestDateTime);
  }

  public static async Task UpdateItemAsyncTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var testDateTime = DateTime.UtcNow;
    var testNameOld = "AuditTest";
    var testNameNew = "AuditTestNew";
    var entityName = nameof(TestAuditEntity);

    // Action.
    var item = new TestAuditData<int>()
    {
      Created = testDateTime,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    // Act.
    var result = await mediator.Send(new TestAuditSaveCommand<int>(item)) as DbSaveResult;

    var allData = (await mediator.Send(new TestAuditGetQuery<int>())).ResultValue;
    var itemId = AuditAssertHelpers.AssertSinglePrimaryKeyWithResult<TestAuditData<int>, int>(result, allData);

    item.Id = itemId;
    item.Name = testNameNew;
    // Update
    await mediator.Send(new TestAuditSaveCommand<int>(item));

    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestAuditEntity, int>(getTableName(entityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().EntityState.Should().Be(AuditStateEnum.Modified);


    var auditItem = resAuditItems.Single(a=>a.EntityState == AuditStateEnum.Modified);
    // only Name was changed
    auditItem.Columns.Should().HaveCount(3);

    var aid = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditEntity.Created)));

    aid.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aid);
    aid.IsChange.Should().BeFalse();
    aid.OldValue.Should().Be(itemId);
    aid.NewValue.Should().BeNull();
    
    aName.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aName);
    aName.IsChange.Should().BeTrue();
    aName.OldValue.Should().NotBeNull();
    aName.NewValue.Should().Be(testNameNew);
    
    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    aCreated.IsChange.Should().BeFalse();
    aCreated.OldValue.Should().NotBeNull();
    aCreated.NewValue.Should().BeNull();
  }

  public static async Task DeleteItemTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var testDateTimeOld = DateTime.UtcNow;
    var testNameOld = "AuditTest";
    var entityName = nameof(TestAuditEntity);

    // Arrange.
    var item = new TestAuditData<int>()
    {
      Created = testDateTimeOld,
      Name = testNameOld,
      NotAuditableColumn = "Audit"
    };

    var res = await mediator.Send(new TestAuditSaveCommand<int>(item)) as DbSaveResult;
    ArgumentNullException.ThrowIfNull(res);
    var itemId = (int)res.ReturnedValues.First().Value.PK;

    // Action.
    var res2 = await mediator.Send(new TestAuditDeleteCommand<int>(itemId));

    // Assert.
    res2.IsSuccess.Should().Be(true);

    var allData = (await mediator.Send(new TestAuditGetQuery<int>())).ResultValue;
    allData.Should().HaveCount(0);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestAuditEntity, int>(getTableName(entityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().EntityState.Should().Be(AuditStateEnum.Deleted);


    var auditItem = resAuditItems.Last();
    auditItem.Columns.Should().HaveCount(3);

    var aid = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditData<int>.Id)));
    var aName = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditData<int>.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(entityName, nameof(TestAuditData<int>.Created)));

    aid.Should().NotBeNull();
    aName.Should().NotBeNull();
    aName!.OldValue.Should().NotBeNull();
    aName.NewValue.Should().BeNull();
    aCreated.Should().NotBeNull();
    aCreated!.OldValue.Should().NotBeNull();
    aCreated.NewValue.Should().BeNull();
  }
}