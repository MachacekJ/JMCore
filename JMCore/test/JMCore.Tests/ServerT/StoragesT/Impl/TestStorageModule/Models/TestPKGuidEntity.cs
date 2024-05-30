﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Base.Audit.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;

[Auditable]
[Table("test_pk_guid")]
public class TestPKGuidEntity
{
  [Key]
  [Column("test_pk_guid_id")]
  public Guid Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = null!;
}