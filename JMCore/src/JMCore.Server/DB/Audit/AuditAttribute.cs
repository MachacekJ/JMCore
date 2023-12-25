namespace JMCore.Server.DB.Audit;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuditableAttribute : Attribute;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class NotAuditableAttribute : Attribute;