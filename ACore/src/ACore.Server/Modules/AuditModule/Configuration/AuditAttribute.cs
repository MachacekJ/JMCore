namespace ACore.Server.Modules.AuditModule.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuditableAttribute(int version) : Attribute
{
  public int Version => version;
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class NotAuditableAttribute : Attribute;