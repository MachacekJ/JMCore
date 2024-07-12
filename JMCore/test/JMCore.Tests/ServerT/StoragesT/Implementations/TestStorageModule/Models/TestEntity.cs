using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.StoragesT.Implemantations.TestStorageModule.Models;

public class TestEntity
{
  [Key]
  public int Id { get; set; }
  
  public string Name { get; set; } = null!;
  
  public DateTime Created { get; set; }
}