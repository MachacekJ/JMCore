using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Models;
// ReSharper disable PropertyCanBeMadeInitOnly.Global


namespace ACore.AppTest.Modules.TestModule.Storages.EF.Models;

internal class TestEntity : IntStorageEntity
{
  public Guid UId { get; init; } = Guid.Empty;
  
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }
}