using ACore.Server.Modules.AuditModule.Configuration;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ACore.Server.Modules.AuditModule.EF;

internal static class AuditDbExtensions
{
  internal static bool IsAuditable(this Type entityEntry, IEnumerable<string>? entities = null)
  {
    var enableAuditAttribute = Attribute.GetCustomAttribute(entityEntry, typeof(AuditableAttribute));

    if (enableAuditAttribute != null)
      return true;

    return entities != null && entities.Contains(entityEntry.Name);
  }

  internal static bool IsAuditable(this Type entityType, string propName, Dictionary<string, IEnumerable<string>>? nonAuditProps)
  {
  
    var propertyInfo = entityType.GetProperty(propName);
    var disableAuditAttribute = propertyInfo != null && Attribute.IsDefined(propertyInfo, typeof(NotAuditableAttribute));
    var isEntityAuditable = entityType.IsAuditable();

    switch (isEntityAuditable)
    {
      case true when disableAuditAttribute:
        return false;
      case true:
        return true;
    }

    if (nonAuditProps == null)
      return false;
    
    if (nonAuditProps.TryGetValue(entityType.Name, out var columns) == false)
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