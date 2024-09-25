﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.AuditModule.Configuration;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

[Auditable(1)]
[Table("test_menu")]
public class TestMenuEntity
{
  [Key]
  [Column("test_menu_id")]
  public int Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = string.Empty;
  
  [Column("last_modify")]
  //[ConcurrencyCheck]
  public DateTime LastModify { get; set; }

  public ICollection<TestCategoryEntity> Categories { get; set; } = [];
}

