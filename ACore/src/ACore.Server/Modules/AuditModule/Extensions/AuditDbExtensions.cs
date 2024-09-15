using ACore.Server.Modules.AuditModule.Configuration;

namespace ACore.Server.Modules.AuditModule.Extensions;

internal static class AuditDbExtensions
{
  internal static AuditableAttribute? IsAuditable(this Type entityEntry)
  {
    var enableAuditAttribute = Attribute.GetCustomAttribute(entityEntry, typeof(AuditableAttribute));
    
    if (enableAuditAttribute is AuditableAttribute auditableAttribute)
      return auditableAttribute;

    return null;
  }

  internal static AuditableAttribute? IsAuditable(this Type entityType, string propName)
  {
    var auditableAttribute = entityType.IsAuditable();
    if (auditableAttribute == null)
      return null;
    
    var propertyInfo = entityType.GetProperty(propName);
    var disableAuditAttribute = propertyInfo != null && Attribute.IsDefined(propertyInfo, typeof(NotAuditableAttribute));

    return disableAuditAttribute ? null : auditableAttribute;
  }
}