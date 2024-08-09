using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Delete;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Get;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Get;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using FluentAssertions;
using MediatR;

namespace ACore.Tests.Server.Modules.AuditModule.Helpers;

public static class AuditCRUDTestHelper
{
  private static readonly DateTime TestDateTime = DateTime.UtcNow;
  private const string TestName = "AuditTest";
  private const string TestNameUpdate = "AuditTestUpdate";
  const string TestAuditEntityName = nameof(TestAuditEntity);
  const string TestNoAuditEntityName = nameof(TestNoAuditEntity);

  public static async Task NoAuditAsyncTest(IMediator mediator, Func<string, string> getTableName)
  {
    // Arrange
    var item = new TestNoAuditData(TestName)
    {
      Created = TestDateTime,
    };

    // Action
    var result = await mediator.Send(new TestNoAuditSaveCommand(item, null));

    // Assert
    var allData = (await mediator.Send(new TestNoAuditGetQuery())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestNoAuditData, int>(result, allData?.Values.ToArray());
    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(TestNoAuditEntityName), itemId))).ResultValue;

    resAuditItems.Should().HaveCount(0);
  }

  public static async Task AddItemAsyncTest<TPK>(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Arrange
    var item = new TestAuditData<TPK>
    {
      Created = TestDateTime,
      Name = TestName,
      NotAuditableColumn = "Audit"
    };

    // Action.
    var result = await mediator.Send(new TestAuditSaveCommand<TPK>(item));


    // Assert.
    var allData = (await mediator.Send(new TestAuditGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestAuditData<TPK>, TPK>(result, allData);
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(TestAuditEntityName), itemId))).ResultValue;
    
    resAuditItems.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(1);
    resAuditItems.Single().State.Should().Be(AuditInfoStateEnum.Added);
    
    var auditItem = resAuditItems.Single();
    auditItem.Columns.Should().HaveCount(6);
    auditItem.Columns.All(c => c.IsChange).Should().Be(true);
    
    var noAuditableColumn = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.NotAuditableColumn)));
    var aid = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Created)));
    
    noAuditableColumn.Should().BeNull();
    
    aid.Should().NotBeNull();
    aName.Should().NotBeNull();
    aName?.NewValue.Should().NotBeNull();
    
    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    aCreated.NewValue.Should().NotBeNull();
    aCreated.DataType.ToLower().Should().NotContain("string");
    
    aid?.NewValue.Should().Be(itemId);
    aName?.NewValue.Should().Be(TestName);
    aCreated.NewValue.Should().Be(TestDateTime);
  }

  public static async Task UpdateItemAsyncTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Action.
    var item = new TestAuditData<int>
    {
      Created = TestDateTime,
      Name = TestName,
      NullValue = TestName,
      NullValue2 = null,
      NotAuditableColumn = "Audit"
    };

    // Act.
    var result = await mediator.Send(new TestAuditSaveCommand<int>(item));

    var allData = (await mediator.Send(new TestAuditGetQuery<int>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestAuditData<int>, int>(result, allData);

    item.Id = itemId;
    item.Name = TestNameUpdate;
    item.NullValue = null;
    item.NullValue2 = TestNameUpdate;

    // Update
    await mediator.Send(new TestAuditSaveCommand<int>(item));

    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(TestAuditEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().State.Should().Be(AuditInfoStateEnum.Modified);


    var auditItem = resAuditItems.Single(a => a.State == AuditInfoStateEnum.Modified);
    // only Name was changed
    auditItem.Columns.Should().HaveCount(6);

    var aid = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Created)));
    var aNullValue = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.NullValue)));
    var aNullValue2 = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.NullValue2)));
    var aNullValue3 = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.NullValue3)));

    aid.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aid);
    aid.IsChange.Should().BeFalse();
    aid.OldValue.Should().Be(itemId);
    aid.NewValue.Should().BeNull();

    aName.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aName);
    aName.IsChange.Should().BeTrue();
    aName.OldValue.Should().Be(TestName);
    aName.NewValue.Should().Be(TestNameUpdate);

    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    aCreated.IsChange.Should().BeFalse();
    aCreated.OldValue.Should().Be(TestDateTime);
    aCreated.NewValue.Should().BeNull();
    aCreated.DataType.ToLower().Should().NotContain("string");

    aNullValue.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue);
    aNullValue.IsChange.Should().BeTrue();
    aNullValue.OldValue.Should().Be(TestName);
    aNullValue.NewValue.Should().BeNull();

    aNullValue2.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue2);
    aNullValue2.IsChange.Should().BeTrue();
    aNullValue2.OldValue.Should().BeNull();
    aNullValue2.NewValue.Should().Be(TestNameUpdate);

    aNullValue3.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue3);
    aNullValue3.IsChange.Should().BeFalse();
    aNullValue3.OldValue.Should().BeNull();
    aNullValue3.NewValue.Should().BeNull();
  }

  public static async Task UpdateItemWithoutChangesAsyncTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Action.
    var item = new TestAuditData<int>
    {
      Created = TestDateTime,
      Name = TestName,
      NullValue = TestName,
      NullValue2 = null,
      NotAuditableColumn = "Audit"
    };

    // Act.
    var result = await mediator.Send(new TestAuditSaveCommand<int>(item));

    var allData = (await mediator.Send(new TestAuditGetQuery<int>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestAuditData<int>, int>(result, allData);

    // Update
    item.Id = itemId;
    await mediator.Send(new TestAuditSaveCommand<int>(item));


    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(TestAuditEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().State.Should().Be(AuditInfoStateEnum.Modified);

    var auditItem = resAuditItems.Single(a => a.State == AuditInfoStateEnum.Modified);
    // only Name was changed
    auditItem.Columns.Should().HaveCount(6);
    auditItem.Columns.All(c => c.IsChange).Should().Be(false);
  }

  public static async Task DeleteItemTest(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Arrange.
    var item = new TestAuditData<int>
    {
      Created = TestDateTime,
      Name = TestName,
      NotAuditableColumn = "Audit",
      NullValue = TestName
    };

    var result = await mediator.Send(new TestAuditSaveCommand<int>(item));
    var allData = (await mediator.Send(new TestAuditGetQuery<int>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestAuditData<int>, int>(result, allData);

    // Action.
    var resultDelete = await mediator.Send(new TestAuditDeleteCommand<int>(itemId));

    // Assert.
    resultDelete.IsSuccess.Should().BeTrue();

    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(TestAuditEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().State.Should().Be(AuditInfoStateEnum.Deleted);


    var auditItem = resAuditItems.Last();
    auditItem.Columns.Should().HaveCount(6);
    auditItem.Columns.All(c => c.IsChange).Should().Be(true);

    var aid = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.Created)));
    var aNullValue = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.NullValue)));
    var aNullValue2 = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.NullValue2)));
    var aNullValue3 = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestAuditEntity.NullValue3)));

    aid.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aid);
    aid.OldValue.Should().Be(itemId);
    aid.NewValue.Should().BeNull();
    aid.DataType.ToLower().Should().NotContain("string");

    aName.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aName);
    aName.OldValue.Should().Be(TestName);
    aName.NewValue.Should().BeNull();

    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    aCreated.OldValue.Should().Be(TestDateTime);
    aCreated.NewValue.Should().BeNull();
    aCreated.DataType.ToLower().Should().NotContain("string");

    aNullValue.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue);
    aNullValue.OldValue.Should().Be(TestName);
    aNullValue.NewValue.Should().BeNull();

    aNullValue2.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue2);
    aNullValue2.OldValue.Should().BeNull();
    aNullValue2.NewValue.Should().BeNull();

    aNullValue3.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue3);
    aNullValue3.OldValue.Should().BeNull();
    aNullValue3.NewValue.Should().BeNull();
  }
}