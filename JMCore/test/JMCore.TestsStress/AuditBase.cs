using System.Diagnostics;
using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;
using JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;

namespace JMCore.TestsStress;

public class AuditBase :  AuditStructureBaseTests
{
  private const int ItemCount = 1;
  protected async Task Audit(StorageTypeEnum storageType)
  {
    var testDb = GetTestStorageModule(storageType);
     
    var testDateTime = DateTime.UtcNow;
    var testNameOld = "AuditTest";
    var testNameNew = "AuditTestNew";

    for (var i = 0; i < ItemCount; i++)
    {
      // Action.
      var item = new TestAttributeAuditEntity()
      {
        Created = testDateTime,
        Name = testNameOld + i,
        NotAuditableColumn = "Audit"
      };
        
      await testDb.AddAsync(item);

      item.Name = testNameNew;
      await testDb.UpdateAsync(item);
      Debug.WriteLine(i);
    }
  }
}