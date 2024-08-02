using System.Reflection;
using ACore.Server.Storages.Models;
using Xunit;

namespace ACore.TestsStress;

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