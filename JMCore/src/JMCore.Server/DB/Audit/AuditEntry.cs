using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JMCore.Server.DB.Audit;

public class AuditEntry
{
    public string TableName { get; }
    public string? SchemaName { get; }
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public EntityState EntityState { get; set; }
    public (string userId, string userName) ByUser { get; }
    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public long? PkValue { get; private set; }
    public string? PkValueString { get; set; }

    private List<PropertyEntry> TemporaryProperties { get; } = new();
    private EntityEntry Entry { get; }

    public AuditEntry(EntityEntry entry, IAuditUserProvider auditUserProvider, IAuditDbConfiguration auditConfiguration)
    {
        Entry = entry;
        PkValue = entry.PrimaryKeyValue();
        PkValueString = entry.PrimaryKeyValueString();

       var tableName = entry.Metadata.GetTableName();
       TableName = tableName ?? throw new ArgumentException($"{nameof(TableName)} is null.");
        
        SchemaName = entry.Metadata.GetSchema();
        EntityState = entry.State;

        ByUser = auditUserProvider.GetUser();

        foreach (var property in entry.Properties)
        {
            if (property.IsAuditable(auditConfiguration.NotAuditProperty))
            {
                if (property.IsTemporary)
                {
                    TemporaryProperties.Add(property);
                    continue;
                }

                var propertyName = property.Metadata.Name;

                switch (entry.State)
                {
                    case EntityState.Added:
                        OldValues[propertyName] = null;
                        NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        OldValues[propertyName] = property.OriginalValue;
                        NewValues[propertyName] = null;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            OldValues[propertyName] = property.OriginalValue;
                            NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                    case EntityState.Unchanged:
                    case EntityState.Detached:
                    default:
                        break;
                }
            }
        }
    }

    public void Update()
    {
        // Get the final value of the temporary properties
        foreach (var prop in TemporaryProperties)
        {
            NewValues[prop.Metadata.Name] = prop.CurrentValue;
        }

        if (!TemporaryProperties.Any(x => x.Metadata.IsKey()))
            return;

        PkValue = Entry.PrimaryKeyValue();
        PkValueString = Entry.PrimaryKeyValueString();
    }
}