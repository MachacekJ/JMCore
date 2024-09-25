using System.ComponentModel.DataAnnotations;
using ACore.Extensions;
using Mapster;

namespace ACore.Server.Storages.Models.PK;

public abstract class PKEntity<TPK>(TPK id)
{
  [Key]
  public TPK Id { get; set; } = id;

  protected static TEntity ToEntity<TEntity>(object data)
    where TEntity: PKEntity<TPK>, new()
  {
    return data.Adapt<TEntity>();
    // var en = new TEntity();
    //
    // var fromProperty = en.GetProperty(nameof(en.Id)) ?? throw new NullReferenceException(nameof(en.Id));
    // var toProperty = data.GetProperty(nameof(en.Id)) ?? throw new NullReferenceException(nameof(en.Id));
    //
    // if (fromProperty.Name != toProperty.Name || fromProperty.PropertyType != toProperty.PropertyType)
    //   throw new Exception($"Property types {fromProperty.PropertyType.Name} and {toProperty.PropertyType.Name} are different.");
    //
    // en.CopyPropertiesFrom(data);
    // return en;
  }
}