using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.Server.Modules.AuditModule.CQRS.Audit;
using ACore.Server.Modules.AuditModule.CQRS.Models;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Modules.TestModule;
using FluentAssertions;
using MongoDB.Bson;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.Mongo;

public class AuditAttribute : MongoAuditTestBase
{
  [Fact]
  public async Task AddItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      // await AuditAttributeTHelper.AddItemAsyncTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop));

      var testDateTime = DateTime.UtcNow;
      var testName = "AuditTest";
      var entityName = nameof(TestAttributeAuditMongoEntity);

      // Action.
      var item = new TestAttributeAuditData<ObjectId>
      {
        Created = testDateTime,
        Name = testName,
        NotAuditableColumn = "Audit"
      };

      var res = await Mediator.Send(new TestAttributeAuditSaveCommand<ObjectId>(item));
      res.Should().NotBeNull();
      res.Should().NotBe(ObjectId.Empty);

      // Assert.
      var allData = await Mediator.Send(new TestAttributeAuditGetQuery<ObjectId>());
      allData.Should().HaveCount(1);

      var savedItem = allData.Single();
      var resAuditItems = await Mediator.Send(new AuditGetQuery<ObjectId>(GetTestTableName(StorageTypeEnum.Mongo, entityName), savedItem.Id));
      resAuditItems.Should().HaveCount(1);
      resAuditItems.Single().EntityState.Should().Be(AuditStateEnum.Added);

      var auditItem = resAuditItems.Single();
      auditItem.Columns.Should().HaveCount(3);

      var aid = auditItem.GetColumn(GetTestColumnName(storageType, entityName, nameof(TestAttributeAuditData<ObjectId>.Id)));
      var aName = auditItem.GetColumn(GetTestColumnName(storageType, entityName, nameof(TestAttributeAuditData<ObjectId>.Name)));
      var aCreated = auditItem.GetColumn(GetTestColumnName(storageType, entityName, nameof(TestAttributeAuditData<ObjectId>.Created)));

      aid.Should().NotBeNull();
      aName.Should().NotBeNull();
      aName!.NewValue.Should().NotBeNull();
      aCreated.Should().NotBeNull();
      aCreated!.NewValue.Should().NotBeNull();

      aid!.NewValue.Should().Be(savedItem.Id);
      aName.NewValue.Should().Be(testName);
      new DateTime(Convert.ToInt64(aCreated.NewValue)).Should().Be(testDateTime);
    });
  }

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await AuditAttributeTHelper.UpdateItemAsyncTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop)); });
  }

  [Fact]
  public async Task DeleteItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await AuditAttributeTHelper.DeleteItemTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop)); });
  }
}