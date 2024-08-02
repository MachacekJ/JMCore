using JMCore.Server.Modules.AuditModule.Configuration;
using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Modules.AuditModule.UserProvider;
using JMCore.Server.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.Modules.AuditModule.Models;

public class AuditEntryItem
{
  private readonly ILogger _logger;
  private readonly IStorage _storage;

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

  public AuditEntryItem(EntityEntry entry, IAuditUserProvider auditUserProvider, IAuditConfiguration auditConfiguration, IStorage storage, ILogger logger)
  {
    _logger = logger;
    _storage = storage;
    Entry = entry;
    PkValue = entry.PrimaryKeyValue();
    PkValueString = entry.PrimaryKeyValueString();

    var tableName = entry.Metadata.GetTableName();
    TableName = tableName ?? throw new ArgumentException($"{nameof(TableName)} is null.");

    SchemaName = entry.Metadata.GetSchema();
    EntityState = entry.State;

    ByUser = auditUserProvider.GetUser();

    CheckAllAuditWithNestedProperties(string.Empty, entry, auditConfiguration);
  }

  private string GetColumnNameInTable(string nestedPrefix, PropertyEntry property)
  {
    var propertyName = $"{nestedPrefix}{property.Metadata.Name}";
    if (_storage.StorageDefinition.DataAnnotationKey == null)
      return propertyName;

    var anno = property.Metadata.GetAnnotation(_storage.StorageDefinition.DataAnnotationKey).Value?.ToString();
    if (anno != null)
      propertyName = anno;

    return propertyName;
  }

  private void CheckAllAuditWithNestedProperties(string nestedPrefix, EntityEntry entry, IAuditConfiguration auditConfiguration)
  {
    foreach (var property in entry.Properties)
    {
      if (!property.IsAuditable(auditConfiguration.NotAuditProperty))
        continue;

      if (property.IsTemporary)
      {
        TemporaryProperties.Add(property);
        continue;
      }

      var propertyName = GetColumnNameInTable(nestedPrefix, property);

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

// https://stackoverflow.com/questions/77052913/ef-core-detect-child-changes-on-parent-at-savechanges
    // foreach (var navigationEntry in entry.Navigations)
    // {
    //   switch (navigationEntry)
    //   {
    //     case CollectionEntry collectionEntry:
    //     {
    //       var i = 0;
    //       if (collectionEntry.CurrentValue != null)
    //         foreach (var referencedEntity in collectionEntry.CurrentValue)
    //         {
    //           var name = $"{nestedPrefix}{navigationEntry.Metadata.Name}[{i}].";
    //           var childEntry = entry.Context.Entry(referencedEntity);
    //           checkedEntries.Add(referencedEntity);
    //           CheckAllAuditWithNestedProperties(name, childEntry, auditConfiguration);
    //           i++;
    //         }
    //
    //       break;
    //     }
    //     case ReferenceEntry referenceEntry:
    //     {
    //       var name = $"{nestedPrefix}{referenceEntry.Metadata.Name}.";
    //       dynamic referencedEntry = referenceEntry.TargetEntry; //entry.Context.Entry(referenceEntry.TargetEntry.Entity);
    //       if (referencedEntry != null)
    //       {
    //         if (!checkedEntries.Any(x => x.Entity.GetType() == referencedEntry.Entity.GetType() && x.Entity.Id == referencedEntry.Entity.Id))
    //         {
    //           CheckAllAuditWithNestedProperties(name, referencedEntry, auditConfiguration);
    //         }
    //         else
    //         {
    //           object entityName = referencedEntry.Entity.GetType();
    //           _logger.LogError("{audit}:Don't use looping data types. Check entity '{entityName}' property name '{name}'.", nameof(AuditEntryItem), entityName.ToString(), name);
    //         }
    //       }
    //
    //       break;
    //     }
    //   }
    // }
  }


  public void Update()
  {
    // Get the final value of the temporary properties
    foreach (var prop in TemporaryProperties)
    {
      var propertyName = GetColumnNameInTable(string.Empty, prop);
      NewValues[propertyName] = prop.CurrentValue;
    }

    if (!TemporaryProperties.Any(x => x.Metadata.IsKey()))
      return;

    PkValue = Entry.PrimaryKeyValue();
    PkValueString = Entry.PrimaryKeyValueString();
  }
}