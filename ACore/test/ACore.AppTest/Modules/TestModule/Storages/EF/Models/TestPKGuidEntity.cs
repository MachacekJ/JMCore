﻿using ACore.AppTest.Modules.TestModule.Models;
using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;

namespace ACore.AppTest.Modules.TestModule.Storages.EF.Models;

// ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable once PropertyCanBeMadeInitOnly.Global
[Auditable]
internal class TestPKGuidEntity : GuidStorageEntity
{
  public string Name { get; set; } = string.Empty;
}

public static class TestPKGuidEntityExtensions
{
  internal static TestPKGuidEntity ToEntity(this TestPKGuidData data)
  {
    var en = new TestPKGuidEntity
    {
      Name = string.Empty
    };
    en.CopyPropertiesFrom(data);
    return en;
  }
}