using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Models;

// ReSharper disable PropertyCanBeMadeInitOnly.Global


namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

internal class TestEntity : IntStorageEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }
}