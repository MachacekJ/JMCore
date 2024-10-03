// using System.Reflection;
// using ACore.Server.Storages.CQRS;
// using ACore.Tests.Server.Storages;
// using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Get;
// using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;
// using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;
// using FluentAssertions;
// using MediatR;
// using Serilog.Sinks.InMemory;
// using Xunit;
//
// namespace ACore.Tests.Server.Modules.TestModule;
//
// public class TestHashTests: StorageTestsBase
// {
//   
//   [Fact]
//   public async Task AllTypes()
//   {
//     var method = MethodBase.GetCurrentMethod();
//     await RunTestAsync(method, async () => await AuditValuesTHelper.AllTypes(Mediator, LogInMemorySink));
//   }
// }
//
// public static class AuditValuesTHelper
// {
//   public static async Task AllTypes(IMediator mediator, InMemorySink logInMemorySink)
//   {
//     var entityName = "TestValueTypeEntity";
//     
//     // Arrange
//     var item = new TestValueTypeData
//     {
//       IntNotNull = int.MaxValue,
//       IntNull = int.MaxValue,
//       BigIntNotNull = long.MaxValue,
//       BigIntNull = long.MaxValue,
//       Bit2 = true,
//       Char2 = "Hello",
//       Date2 = DateTime.Today.ToUniversalTime(),
//       DateTime2 = DateTime.UtcNow,
//       Decimal2 = 12345678901.12345678M,
//       NChar2 = "Čau říá",
//       NVarChar2 = "říkám já",
//       SmallDateTime2 = new DateTime(2000, 10, 10, 10, 10, 0, DateTimeKind.Utc),
//       SmallInt2 = short.MaxValue,
//       TinyInt2 = byte.MaxValue,
//       Guid2 = Guid.NewGuid(),
//       VarBinary2 = new byte[10],
//       VarChar2 = "říkám já řřČŘÉÍÁ"
//     };
//
//     // Act.
//     var result = await mediator.Send(new TestValueTypeSaveCommand(item)) as DbSaveResult;
//     
//     // Assert
//     ArgumentNullException.ThrowIfNull(result);
//     var allData = (await mediator.Send(new TestValueTypeGetQuery())).ResultValue;
//     ArgumentNullException.ThrowIfNull(allData);
//     allData.Should().HaveCount(1);
//     item = allData[0];
//     item.VarChar2 = "test";
//    
//     
//     await mediator.Send(new TestValueTypeSaveCommand(item));
//     
//   }
// }
