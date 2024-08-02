using System.Reflection;
using ACore.Server.Storages.Models;
using Xunit;

namespace ACore.TestsStress;

public class AuditMongo : AuditBase
{
  protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await Audit(storageType); });
  }
}