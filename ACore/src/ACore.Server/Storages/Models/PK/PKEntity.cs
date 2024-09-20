using System.ComponentModel.DataAnnotations;

namespace ACore.Server.Storages.Models.PK;

public abstract class PKEntity<T>(T id)
{
  [Key]
  public T Id { get; set; } = id;
}