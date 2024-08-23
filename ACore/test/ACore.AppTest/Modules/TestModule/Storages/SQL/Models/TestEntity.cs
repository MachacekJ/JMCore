using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;

// ReSharper disable PropertyCanBeMadeInitOnly.Global


namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

internal class TestEntity : PKIntEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }
}