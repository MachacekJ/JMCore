using System.Reflection;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Storages.CQRS;
using ACore.Tests.Server.Modules.AuditModule.Helpers;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Get;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using FluentAssertions;
using MediatR;
using Xunit;

namespace ACore.Tests.Server.Modules.AuditModule;

/// <summary>
/// Two kinds of table primary key are supported.
/// ObjectId for mongoDb is tested in integration tests. 
/// </summary>
// ReSharper disable once InconsistentNaming
public class AuditPKTests : AuditTestsBase
{
  [Fact]
  public async Task IntPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.IntPK(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task LongPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.LongPK(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task GuidPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.GuidPK(Mediator, GetTableName, GetColumnName); });
  }

  [Fact]
  public async Task StringPKTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await AuditPKTestHelper.StringPK(Mediator, GetTableName, GetColumnName); });
  }
  
  [Fact]
  public async Task ObjectIdPKNotImplTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Mongo entity with pk is not implemented in memory storage.
      await AuditPKTestHelper.ObjectIdPKNotImplemented(Mediator);
    });
  }
}