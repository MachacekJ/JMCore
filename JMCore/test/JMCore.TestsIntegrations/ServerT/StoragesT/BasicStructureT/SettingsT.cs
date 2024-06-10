﻿using System.Reflection;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.BasicStructureT;

public class SettingsT : BasicStructureBaseT
{
  [Fact]
  public async Task SaveGetTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var db = GetBasicStorageModule(storageType);
      await Tests.ServerT.StoragesT.BasicStorageT.SettingsT.CheckSettingEntity(db, Mediator); 
    });
  }
}