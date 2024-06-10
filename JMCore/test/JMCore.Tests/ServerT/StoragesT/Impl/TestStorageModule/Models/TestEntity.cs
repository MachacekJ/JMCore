using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;

public class TestEntity
{
  [Key]
  public int Id { get; set; }
  
  public string Name { get; set; } = null!;
  
  public DateTime Created { get; set; }
}