namespace JMCore.Server.Storages.Base.Audit.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuditableAttribute : Attribute;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class NotAuditableAttribute : Attribute;