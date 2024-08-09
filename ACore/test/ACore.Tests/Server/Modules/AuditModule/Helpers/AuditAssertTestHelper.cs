using ACore.Base.CQRS.Results;
using ACore.Extensions;
using ACore.Server.Storages.CQRS;
using FluentAssertions;

namespace ACore.Tests.Server.Modules.AuditModule.Helpers;

public static class AuditAssertTestHelper
{
  public static TPK AssertSinglePrimaryKeyWithResult<T, TPK>(Result? result, T[]? data)
    where T : class
  {
    ArgumentNullException.ThrowIfNull(result);
    ArgumentNullException.ThrowIfNull(data);

    result.Should().BeOfType<DbSaveResult>();
    var dbSaveResult = (DbSaveResult)result;
    dbSaveResult.IsSuccess.Should().BeTrue();
    dbSaveResult.Should().NotBeNull();
    dbSaveResult.ReturnedValues.Should().HaveCount(1);
    data.Should().HaveCount(1);

    var pk = dbSaveResult.PrimaryKeySingle<TPK>();
    var pkData = Convert.ChangeType(data.First().PropertyValue("Id"), typeof(TPK));
    pk.Should().Be(pkData);

    return pk;
  }
}