// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

internal class TestManualAuditEntity : PKLongEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;
  
  [MaxLength(200)]
  public string NotAuditableColumn { get; set; } = string.Empty;
  
  public DateTime Created { get; set; }
}