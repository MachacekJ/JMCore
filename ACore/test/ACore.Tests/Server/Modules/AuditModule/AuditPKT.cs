﻿using System.Reflection;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;
using FluentAssertions;
using MediatR;
using Xunit;

namespace ACore.Tests.Server.Modules.AuditModule;

/// <summary>
/// Two kinds of table primary key are supported. 
/// </summary>
// ReSharper disable once InconsistentNaming
public class AuditPKT : AuditAttributeBase
{
  [Fact]
  public async Task GuidPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.GuidPK(Mediator, GetTableName); });
  }

  [Fact]
  public async Task StringPK()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTHelper.StringPK(Mediator, GetTableName); });
  }
}

// ReSharper disable once InconsistentNaming
public static class AuditPKTHelper
{
  private const string TestName = "AuditPK";
  private const string TestPKGuidEntityName = "TestPKGuidEntity";
  private const string TestPKStringEntityName = "TestPKStringEntity";

  public static async Task GuidPK(IMediator mediator, Func<string, string> getTableName)
  {
    // Arrange.
    var item = new TestPKGuidData()
    {
      Name = TestName,
    };

    // Action 1
    await mediator.Send(new TestPKGuidSaveCommand(item));
    item.Id.Should().NotBeEmpty();

    // Assert 1
    var allData = (await mediator.Send(new TestPKGuidGetQuery())).ResultValue;
    allData.Should().HaveCount(1);

    // var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKGuidEntityName), item.Id.ToString());
    // var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    // auditVwAuditEntities.Count().Should().Be(2);
    // auditVwAuditEntities.Single(a => a.NewValueGuid == item.Id).NewValueGuid.Should().Be(item.Id);
    //
    // // Arrange 2
    // item.Name = TestName + "2";
    //
    // // Action 2 
    // var res2 = await mediator.Send(new TestPKGuidSaveCommand(item)); 
    // res2.Should().Be(true);
    //
    //
    // // Assert 2
    // auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKGuidEntityName), item.Id.ToString());
    // auditValues.Count(a => a.PKValueString == item.Id.ToString()).Should().Be(3);
  }

  public static async Task StringPK(IMediator mediator, Func<string, string> getTableName)
  {
    // Arrange.
    var item = new TestPKStringData()
    {
      Name = TestName,
    };

    // Action 1
    await mediator.Send(new TestPKStringSaveCommand(item));
    item.Id.Should().NotBeEmpty();

    // Assert 1
    var allData = (await mediator.Send(new TestPKStringGetQuery())).ResultValue;
    allData.Should().HaveCount(1);

    // var auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKStringEntityName), item.Id);
    // var auditVwAuditEntities = auditValues as AuditVwAuditEntity[] ?? auditValues.ToArray();
    // auditVwAuditEntities.Count().Should().Be(2);
    // auditVwAuditEntities.Single(a => a.NewValueString == item.Id).NewValueString.Should().Be(item.Id);
    //
    // // Arrange 2
    // item.Name = TestName + "2";
    //
    // // Action 2 
    // var res2 = await mediator.Send(new TestPKStringSaveCommand(item)); 
    // res2.Should().Be(true);
    //
    // // Assert 2
    // auditValues = await auditStorageModule.AuditItemsAsync(getTableName(TestPKStringEntityName), item.Id);
    // auditValues.Count(a => a.PKValueString == item.Id).Should().Be(3);
  }
}