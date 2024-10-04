using ACore.Base.CQRS.Models.Results;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;

namespace ACore.Tests.Server.Modules.AuditModule.Helpers;

public static class AuditPKTestHelper
{
  private const string TestName = "AuditPK";
  private const string TestPKGuidEntityName = nameof(TestPKGuidEntity);
  private const string TestPKStringEntityName = nameof(TestPKStringEntity);
  private const string TestPKLongEntityName = nameof(TestPKLongEntity);
  private const string TestPKMongoEntityName = nameof(TestPKMongoEntity);
  private const string TestAuditEntityName = nameof(TestAuditEntity);

  public static async Task IntPK(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Arrange.
    var item = new TestAuditData<int>
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new TestAuditSaveCommand<int>(item)) as DbSaveResult;

    // Assert.
    var allData = (await mediator.Send(new TestAuditGetQuery<int>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestAuditData<int>, int>(result, allData);
    
 
    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestAuditEntity, int>(getTableName(TestAuditEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestPKLongEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }
  
  public static async Task LongPK(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Arrange.
    var item = new TestPKLongData
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new TestPKLongSaveCommand(item)) as DbSaveResult;

    // Assert.
    var allData = (await mediator.Send(new TestPKLongAuditGetQuery())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestPKLongData, long>(result, allData);
    
 
    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestPKLongEntity, long>(getTableName(TestPKLongEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestPKStringEntityName, nameof(TestPKLongEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }
  
  public static async Task GuidPK(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Arrange.
    var item = new TestPKGuidData
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new TestPKGuidSaveCommand(item)) as DbSaveResult;

    // Assert.
    var allData = (await mediator.Send(new TestPKGuidGetQuery())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestPKGuidData, Guid>(result, allData);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestPKGuidEntity, Guid>(getTableName(TestPKGuidEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestPKGuidEntityName, nameof(TestPKGuidEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }

  public static async Task StringPK(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    // Arrange.
    var item = new TestPKStringData
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new TestPKStringSaveCommand(item)) as DbSaveResult;

    // Assert.
    var allData = (await mediator.Send(new TestPKStringGetQuery())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestPKStringData, string>(result, allData);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestPKStringEntity, string>(getTableName(TestPKStringEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestPKStringEntityName, nameof(TestPKStringEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }

  public static async Task ObjectIdPKNotImplemented(IMediator mediator)
  {
    var item = new TestAuditData<ObjectId>
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new TestAuditSaveCommand<ObjectId>(item));
    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be(ACore.Base.CQRS.Models.Results.Error.Error.ErrorInternalServer.Code);
    result.Should().BeOfType(typeof(ExceptionResult));
  }

  public static async Task ObjectIdPK(IMediator mediator, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var item = new TestAuditData<ObjectId>
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new TestAuditSaveCommand<ObjectId>(item)) as DbSaveResult;

    // Assert.
    var allData = (await mediator.Send(new TestAuditGetQuery<ObjectId>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestAuditData<ObjectId>, ObjectId>(result, allData);
    
 
    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TestPKMongoEntity, ObjectId>(getTableName(TestPKMongoEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(TestPKMongoEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }
}