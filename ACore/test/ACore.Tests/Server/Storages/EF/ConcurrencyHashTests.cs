using System.Reflection;
using ACore.Server.Storages.CQRS;
using ACore.Tests.Server.Modules.AuditModule;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Server.Storages.EF;

public class AuditCheckSumTests : AuditTestsBase
{
  private static readonly DateTime TestDateTime = new DateTime(2024,1,2).ToUniversalTime();
  private const string TestName = "AuditTest";

  [Fact]
  public async Task HashCorrectTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange
      var item = new TestNoAuditData(TestName)
      {
        Created = TestDateTime,
      };

      // Action
      var result = (await Mediator.Send(new TestNoAuditSaveCommand(item, null))) as DbSaveResult;

      ArgumentNullException.ThrowIfNull(result);
      var hash = result.HashSingle();
      item.Id = result.PrimaryKeySingle<int>();
   
      var result2 = (await Mediator.Send(new TestNoAuditSaveCommand(item, hash))) as DbSaveResult;
      var hash2 = result2?.HashSingle();
      
      var allData = (await Mediator.Send(new TestNoAuditGetQuery())).ResultValue;
      ArgumentNullException.ThrowIfNull(allData);

      allData.Should().HaveCount(1);
      var savedItem = allData.Single();
      savedItem.Key.Should().Be(hash2).And.Be(hash);
      
      item.Name = "faketest";
      var result3 = (await Mediator.Send(new TestNoAuditSaveCommand(item, hash))) as DbSaveResult;
      var hash3 = result3?.HashSingle();
      savedItem.Key.Should().NotBe(hash3);
      
      
    });
  }
  
  [Fact]
  public async Task HashNullTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange
      var item = new TestNoAuditData(TestName)
      {
        Created = TestDateTime,
      };

      // Action
      var result = (await Mediator.Send(new TestNoAuditSaveCommand(item, null))) as DbSaveResult;

      ArgumentNullException.ThrowIfNull(result);
      var hash = result.HashSingle();
      item.Id = result.PrimaryKeySingle<int>();
   
      var result2 = (await Mediator.Send(new TestNoAuditSaveCommand(item, null))) as DbSaveResult;

      
     
      
      
    });
  }
  
}