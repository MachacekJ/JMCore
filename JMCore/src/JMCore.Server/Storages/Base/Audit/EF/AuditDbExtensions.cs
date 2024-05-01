using JMCore.Server.Storages.Base.Audit.Configuration;
using JMCore.Server.Storages.Modules.AuditModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JMCore.Server.Storages.Base.Audit.EF;

internal static class AuditDbExtensions
{
  internal static bool ShouldBeAudited(this EntityEntry entry, IEnumerable<string> tables)
  {
    return entry.State != EntityState.Detached && entry.State != EntityState.Unchanged &&
           !(entry.Entity is AuditEntity) && entry.IsAuditable(tables);
  }

  private static bool IsAuditable(this EntityEntry entityEntry, IEnumerable<string>? entities = null)
  {
    var enableAuditAttribute = Attribute.GetCustomAttribute(entityEntry.Entity.GetType(), typeof(AuditableAttribute));

    if (enableAuditAttribute != null)
      return true;

    return entities != null && entities.Contains(entityEntry.Entity.GetType().Name);
  }

  internal static bool IsAuditable(this PropertyEntry propertyEntry, Dictionary<string, IEnumerable<string>> nonAuditProps)
  {
    var entityType = propertyEntry.EntityEntry.Entity.GetType();
    var propertyInfo = entityType.GetProperty(propertyEntry.Metadata.Name);
    var disableAuditAttribute = propertyInfo != null && Attribute.IsDefined(propertyInfo, typeof(NotAuditableAttribute));
    var isEntityAuditable = propertyEntry.EntityEntry.IsAuditable();
    //var res = && !disableAuditAttribute;

    switch (isEntityAuditable)
    {
      case true when disableAuditAttribute:
        return false;
      case true:
        return true;
    }

    if (nonAuditProps.TryGetValue(propertyEntry.EntityEntry.Entity.GetType().Name, out var columns) == false)
      return true;

    return propertyInfo != null && !columns.Contains(propertyInfo.Name);
  }

  public static long? PrimaryKeyValue(this EntityEntry entry)
  {
    var primaryKey = entry.Metadata.FindPrimaryKey();

    var firstProp = primaryKey?.Properties.FirstOrDefault();
    if (firstProp == null || firstProp.PropertyInfo == null)
      return null;

    var rr = firstProp.PropertyInfo.GetValue(entry.Entity);
    if (rr == null)
      return null;

    return long.TryParse(rr.ToString(), out var pkValue) ? pkValue : null;
  }

  public static string? PrimaryKeyValueString(this EntityEntry entry)
  {
    var primaryKey = entry.Metadata.FindPrimaryKey();

    var firstProp = primaryKey?.Properties.FirstOrDefault();
    if (firstProp == null || firstProp.PropertyInfo == null)
      return null;

    var rr = firstProp.PropertyInfo.GetValue(entry.Entity);

    return rr?.ToString();
  }
}