using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Base.Audit.Configuration;

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;

[Auditable]
[Table("test_pk_string")]
public class TestPKStringEntity
{
  [Key]
  [Column("test_pk_string_id")]
  [MaxLength(50)]
  public string Id { get; set; } = null!;

  [Column("name")]
  [MaxLength(20)]
  public string Name { get; set; } = null!;
}