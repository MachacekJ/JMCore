using ACore.Extensions;
using ACore.Server.Storages.CQRS;
using FluentAssertions;

namespace ACore.Tests.Server.Modules.AuditModule.Helpers;

public static class AuditAssertHelpers
{
  public static TPK AssertSinglePrimaryKeyWithResult<T, TPK>(DbSaveResult? result, T[]? data)
    where T : class
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(data);
    
    result.Should().NotBeNull();
    result.ReturnedValues.Should().HaveCount(1);
    data.Should().HaveCount(1);

    var pk = result.PrimaryKeySingle<TPK>();
    var pkData = Convert.ChangeType(data.First().PropertyValue("Id"), typeof(TPK));
    pk.Should().Be(pkData);

    return pk;
  }
}