﻿using System.ComponentModel.DataAnnotations;
using ACore.Extensions;
using ACore.Server.Storages.Attributes;
using ACore.Server.Storages.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

[CheckSum]
internal class TestNoAuditEntity : PKIntEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  public static TestNoAuditEntity Create(TestNoAuditData data)
    => ToEntity<TestNoAuditEntity>(data);
}
