using System.Reflection;
using System.Text.Json;
using FluentAssertions;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.AuditModule.Helper;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;
using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;

/// <summary>
/// Test for different C# types and their persistence.
/// </summary>
public class AuditValuesT : AuditAttributeBaseT
{
  [Fact]
  public async Task AllTypes()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => await AuditValuesTHelper.AllTypes(AuditStorageModule, TestStorageModule, LogInMemorySink, (name) => name));
  }
}

public static class AuditValuesTHelper
{
  public static async Task AllTypes(IAuditStorageModule auditStorageModule, ITestStorageModule testStorageEfContext, InMemorySink logInMemorySink, Func<string, string> getTableName)
  {
    // Arrange
    var item = new TestValueTypeEntity
    {
      IntNotNull = int.MaxValue,
      IntNull = int.MaxValue,
      BigIntNotNull = long.MaxValue,
      BigIntNull = long.MaxValue,
      Bit2 = true,
      Char2 = "Hello",
      Date2 = DateTime.Today.ToUniversalTime(),
      DateTime2 = DateTime.UtcNow,
      Decimal2 = 12345678901.12345678M,
      NChar2 = "Čau říá",
      NVarChar2 = "říkám já",
      SmallDateTime2 = new DateTime(2000, 10, 10, 10, 10, 0, DateTimeKind.Utc),
      SmallInt2 = short.MaxValue,
      TinyInt2 = byte.MaxValue,
      Guid2 = Guid.NewGuid(),
      VarBinary2 = new byte[AuditValueConverterHelper.MaxStringSize],
      VarChar2 = "říkám já řřČŘÉÍÁ"
    };
    await testStorageEfContext.AddAsync(item);

    logInMemorySink.Should().HaveMessage("The value exceeded the maximum character length '{MaxStringSize}'. Value:{Value}")
      .Appearing().Once().WithLevel(LogEventLevel.Error);

    var auditItem = (await auditStorageModule.AllAuditItemsAsync(getTableName(nameof(TestValueTypeEntity)))).Where(a => a.EntityState == EntityState.Added).ToList();

    // 17 fields + 1 Id
    auditItem.Should().HaveCount(18);
    auditItem.Single(a => a.ColumnName == "Id").NewValueInt.Should().Be(item.Id);
    auditItem.Single(a => a.ColumnName == "IntNotNull").NewValueInt.Should().Be(item.IntNotNull);
    auditItem.Single(a => a.ColumnName == "IntNull").NewValueInt.Should().Be(item.IntNull);
    auditItem.Single(a => a.ColumnName == "BigIntNotNull").NewValueLong.Should().Be(item.BigIntNotNull);
    auditItem.Single(a => a.ColumnName == "BigIntNull").NewValueLong.Should().Be(item.BigIntNull);
    auditItem.Single(a => a.ColumnName == "Bit2").NewValueBool.Should().Be(item.Bit2);
    auditItem.Single(a => a.ColumnName == "Char2").NewValueString.Should().Be(item.Char2);
    auditItem.Single(a => a.ColumnName == "Date2").NewValueLong.Should().Be(item.Date2.Ticks);
    auditItem.Single(a => a.ColumnName == "DateTime2").NewValueLong.Should().Be(item.DateTime2.Ticks);
    auditItem.Single(a => a.ColumnName == "Decimal2").NewValueString.Should().Be(JsonSerializer.Serialize(item.Decimal2));
    auditItem.Single(a => a.ColumnName == "NChar2").NewValueString.Should().Be(item.NChar2);
    auditItem.Single(a => a.ColumnName == "NVarChar2").NewValueString.Should().Be(item.NVarChar2);
    auditItem.Single(a => a.ColumnName == "SmallDateTime2").NewValueLong.Should().Be(item.SmallDateTime2.Ticks);
    auditItem.Single(a => a.ColumnName == "SmallInt2").NewValueInt.Should().Be(item.SmallInt2);
    auditItem.Single(a => a.ColumnName == "TinyInt2").NewValueInt.Should().Be(item.TinyInt2);
    auditItem.Single(a => a.ColumnName == "Guid2").NewValueGuid.Should().Be(item.Guid2);
    auditItem.Single(a => a.ColumnName == "VarBinary2").NewValueString.Should().Be(JsonSerializer.Serialize(item.VarBinary2));
    auditItem.Single(a => a.ColumnName == "VarChar2").NewValueString.Should().Be(item.VarChar2);
  }
}