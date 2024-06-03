// using System.Reflection;
// using FluentAssertions;
// using JMCore.Tests.ServerT.DbT.TestDBContext.Models;
// using Microsoft.EntityFrameworkCore;
// using Serilog.Events;
// using Serilog.Sinks.InMemory.Assertions;
// using Xunit;
//
// namespace JMCore.TestsIntegrations.ServerT.DbT.AuditStructureT;
//
// public class AuditValuesT : AuditStructureBaseT
// {
//     [Fact]
//     public async Task AllTypes()
//     {
//         var method = MethodBase.GetCurrentMethod();
//         await RunTestAsync(method, async () =>
//         {
//             // Arrange
//             var item = new TestValueTypeEntity
//             {
//                 IntNotNull = int.MaxValue,
//                 IntNull = int.MaxValue,
//                 BigIntNotNull = long.MaxValue,
//                 BigIntNull = long.MaxValue,
//                 Bit2 = true,
//                 Char2 = "Hello",
//                 Date2 = DateTime.Today.ToUniversalTime(),
//                 DateTime2 = DateTime.UtcNow,
//                 Decimal2 = 12345678901.12345678M,
//                 NChar2 = "Čau říá",
//                 NVarChar2 = "říkám já",
//                 SmallDateTime2 = new DateTime(2000, 10, 10, 10, 10, 0, DateTimeKind.Utc),
//                 SmallInt2 = short.MaxValue,
//                 TinyInt2 = byte.MaxValue,
//                 Guid2 = Guid.NewGuid(),
//                 VarBinary2 = new byte[AuditDbContext.MaxStringSize],
//                 VarChar2 = "říkám já řřČŘÉÍÁ"
//             };
//             await TestBasicDbContext.TestValueTypes.AddAsync(item);
//             await TestBasicDbContext.SaveChangesAsync();
//
//             LogInMemorySink.Should().HaveMessage("The value exceeded the maximum character length '{MaxStringSize}'. Value:{Value}")
//                 .Appearing().Once().WithLevel(LogEventLevel.Error);
//
//             var auditItem = AuditDbContext.VwAudits().Where(a => a.TableName == "test_value_type" && a.EntityState == EntityState.Added).ToList();
//             
//             // 17 fields + 1 Id
//             auditItem.Should().HaveCount(18);
//             auditItem.Single(a => a.ColumnName == "Id").NewValueInt.Should().Be(item.Id);
//             auditItem.Single(a => a.ColumnName == "IntNotNull").NewValueInt.Should().Be(item.IntNotNull);
//             auditItem.Single(a => a.ColumnName == "IntNull").NewValueInt.Should().Be(item.IntNull);
//             auditItem.Single(a => a.ColumnName == "BigIntNotNull").NewValueLong.Should().Be(item.BigIntNotNull);
//             auditItem.Single(a => a.ColumnName == "BigIntNull").NewValueLong.Should().Be(item.BigIntNull);
//             auditItem.Single(a => a.ColumnName == "Bit2").NewValueBool.Should().Be(item.Bit2);
//             auditItem.Single(a => a.ColumnName == "Char2").NewValueString.Should().Be(item.Char2);
//             auditItem.Single(a => a.ColumnName == "Date2").NewValueLong.Should().Be(item.Date2.Ticks);
//             auditItem.Single(a => a.ColumnName == "DateTime2").NewValueLong.Should().Be(item.DateTime2.Ticks);
//             auditItem.Single(a => a.ColumnName == "Decimal2").NewValueString.Should().Be(Newtonsoft.Json.JsonConvert.SerializeObject(item.Decimal2));
//             auditItem.Single(a => a.ColumnName == "NChar2").NewValueString.Should().Be(item.NChar2);
//             auditItem.Single(a => a.ColumnName == "NVarChar2").NewValueString.Should().Be(item.NVarChar2);
//             auditItem.Single(a => a.ColumnName == "SmallDateTime2").NewValueLong.Should().Be(item.SmallDateTime2.Ticks);
//             auditItem.Single(a => a.ColumnName == "SmallInt2").NewValueInt.Should().Be(item.SmallInt2);
//             auditItem.Single(a => a.ColumnName == "TinyInt2").NewValueInt.Should().Be(item.TinyInt2);
//             auditItem.Single(a => a.ColumnName == "Guid2").NewValueGuid.Should().Be(item.Guid2);
//             auditItem.Single(a => a.ColumnName == "VarBinary2").NewValueString.Should().Be(Newtonsoft.Json.JsonConvert.SerializeObject(item.VarBinary2));
//             auditItem.Single(a => a.ColumnName == "VarChar2").NewValueString.Should().Be(item.VarChar2);
//         });
//     }
// }