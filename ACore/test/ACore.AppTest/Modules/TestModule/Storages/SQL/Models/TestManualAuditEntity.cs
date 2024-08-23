// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Models;

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

internal class TestManualAuditEntity : LongStorageEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;
  
  [MaxLength(200)]
  public string NotAuditableColumn { get; set; } = string.Empty;
  
  public DateTime Created { get; set; }
}