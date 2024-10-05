using System.Reflection;
using ACore.Tests.Server.Modules.AuditModule.Helpers;
using Xunit;

namespace ACore.Tests.Server.Modules.AuditModule.PG;

/// <summary>
/// Test for different C# types and their persistence.
/// </summary>
public class AuditAllDataTypesTests : AuditTestsBase
{
  [Fact]
  public async Task AllDataTypesTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => await AuditAllDataTypesTestHelper.AllDataTypes(Mediator, LogInMemorySink, GetTableName, GetColumnName));
  }
}
