using System.Reflection;
using JMCore.Server.Storages.Models;
using Xunit;

namespace JMCore.TestsStress;

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