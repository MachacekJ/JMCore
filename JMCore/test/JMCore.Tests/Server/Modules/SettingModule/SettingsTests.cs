﻿using System.Reflection;
using FluentAssertions;
using JMCore.Modules.CacheModule;
using JMCore.Modules.CacheModule.CQRS.CacheGet;
using JMCore.Modules.CacheModule.CQRS.Models;
using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Modules.SettingModule.Storage.Models;
using JMCore.Server.Services.JMCache;
using MediatR;
using Xunit;

namespace JMCore.Tests.Server.Modules.SettingModule;

public class SettingsTests : BasicStorageModuleEfContextTests
{
  [Fact]
  public async Task SaveGetTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () => { await CheckSettingEntity(Db, Mediator); });
  }

 
#pragma warning disable xUnit1013
  public static async Task CheckSettingEntity(IBasicStorageModule db, IMediator mediator)
#pragma warning restore xUnit1013
  {
    string key = "key";
    string key2 = "key2";
    string value = "value";
    string value2 = "value2";
    await db.Setting_SaveAsync(key, value, true);
    var val = await db.Setting_GetAsync(key);
    val.Should().Be(value);

    await db.Setting_SaveAsync(key, value2, true);
    var val2 = await db.Setting_GetAsync(key);
    val2.Should().Be(value2);

    // Check if is value in cache
    var keyCache = JMCacheKey.Create(JMCacheServerCategory.DbTable, nameof(SettingEntity));
    var cacheValue = await mediator.Send(new CacheModuleGetQuery(keyCache));
    var mem = cacheValue!.Value as List<SettingEntity>;
    Assert.True(mem != null && mem.First(a => a.Key == key).Value == value2);

    Exception? isError = null;
    try
    {
      await db.Setting_GetAsync(key2);
    }
    catch (Exception ex)
    {
      isError = ex;
    }

    Assert.True(isError != null);
  }
}