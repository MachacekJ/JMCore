using System.Reflection;
using JMCore.Server.Storages.Models;
using Xunit;

namespace JMCore.TestsStress;

public class AuditPG: AuditBase
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await Audit(storageType); });
  }


}