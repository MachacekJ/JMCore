using System.ComponentModel.DataAnnotations;
using Mapster;

namespace ACore.Server.Storages.Definitions.Models.PK;

public abstract class PKEntity<TPK>(TPK id)
{
  [Key]
  public TPK Id { get; set; } = id;

  protected static TEntity ToEntity<TEntity>(object data)
    where TEntity: PKEntity<TPK>, new()
  {
    return data.Adapt<TEntity>();
  }
}