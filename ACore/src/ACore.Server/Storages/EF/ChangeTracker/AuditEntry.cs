// using ACore.Server.Modules.AuditModule.Configuration;
// using ACore.Server.Modules.AuditModule.Extensions;
// using ACore.Server.Storages.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.ChangeTracking;
// using Microsoft.EntityFrameworkCore.Metadata;
//
// namespace ACore.Server.Storages.EF.ChangeTracker;
//
// public class AuditEntry
// {
//   private readonly StorageTypeDefinition _storageTypeDefinition;
//   private readonly IAuditConfiguration _auditConfiguration;
//   public string TableName { get; }
//   public string? SchemaName { get; }
//   public Dictionary<string, object?> OldValues { get; } = new();
//   public Dictionary<string, object?> NewValues { get; } = new();
//
//   public EntityState EntityState { get; set; }
//
//   //public (string userId, string userName) ByUser { get; }
//   public bool HasTemporaryProperties => TemporaryProperties.Any();
//
//   public long? PkValue { get; private set; }
//   public string? PkValueString { get; set; }
//
//   private List<PropertyEntry> TemporaryProperties { get; } = new();
//   private EntityEntry Entry { get; }
//
//   public AuditEntry(EntityEntry entry, IAuditConfiguration auditConfiguration, StorageTypeDefinition storageTypeDefinition)
//   {
//     _storageTypeDefinition = storageTypeDefinition;
//     _auditConfiguration = auditConfiguration;
//
//     Entry = entry;
//     PkValue = entry.PrimaryKeyValue();
//     PkValueString = entry.PrimaryKeyValueString();
//
//     TableName = GetTableName(entry.Metadata) ?? throw new ArgumentException($"{nameof(TableName)} is null.");
//
//     SchemaName = entry.Metadata.GetSchema();
//     EntityState = entry.State;
//
//     //ByUser = auditUserProvider.GetUser();
//
//     CheckAllAuditWithNestedProperties(string.Empty, entry);
//
//     // foreach (var property in entry.Properties)
//     // {
//     //     if (entry.GetType().IsAuditable(property.Metadata.Name, auditConfiguration.NotAuditProperty))
//     //     {
//     //         if (property.IsTemporary)
//     //         {
//     //             TemporaryProperties.Add(property);
//     //             continue;
//     //         }
//     //
//     //         var propertyName = property.Metadata.Name;
//     //
//     //         switch (entry.State)
//     //         {
//     //             case EntityState.Added:
//     //                 OldValues[propertyName] = null;
//     //                 NewValues[propertyName] = property.CurrentValue;
//     //                 break;
//     //             case EntityState.Deleted:
//     //                 OldValues[propertyName] = property.OriginalValue;
//     //                 NewValues[propertyName] = null;
//     //                 break;
//     //
//     //             case EntityState.Modified:
//     //                 if (property.IsModified)
//     //                 {
//     //                     OldValues[propertyName] = property.OriginalValue;
//     //                     NewValues[propertyName] = property.CurrentValue;
//     //                 }
//     //                 break;
//     //             case EntityState.Unchanged:
//     //             case EntityState.Detached:
//     //             default:
//     //                 break;
//     //         }
//     //     }
//     // }
//   }
//
//   public void Update()
//   {
//     // Get the final value of the temporary properties
//     foreach (var prop in TemporaryProperties)
//     {
//       NewValues[prop.Metadata.Name] = prop.CurrentValue;
//     }
//
//     if (!TemporaryProperties.Any(x => x.Metadata.IsKey()))
//       return;
//
//     PkValue = Entry.PrimaryKeyValue();
//     PkValueString = Entry.PrimaryKeyValueString();
//   }
//
//
//   private string GetColumnNameInTable(string nestedPrefix, PropertyEntry property)
//   {
//     var propertyName = $"{nestedPrefix}{property.Metadata.Name}";
//
//     if (_storageTypeDefinition.DataAnnotationColumnNameKey == null)
//       return propertyName;
//
//     try
//     {
//       var anno = property.Metadata.GetAnnotation(_storageTypeDefinition.DataAnnotationColumnNameKey).Value?.ToString();
//       if (anno != null)
//         propertyName = $"{nestedPrefix}{anno}";
//     }
//     catch (Exception)
//     {
//       // no correct annotation has not been found - ignored
//     }
//
//     return propertyName;
//   }
//
//   private void CheckAllAuditWithNestedProperties(string nestedPrefix, EntityEntry entry)
//   {
//     foreach (var property in entry.Properties)
//     {
//       if (entry.GetType().IsAuditable(property.Metadata.Name, _auditConfiguration.NotAuditProperty))
//         continue;
//
//       if (property.IsTemporary)
//       {
//         TemporaryProperties.Add(property);
//         continue;
//       }
//
//       var propertyName = GetColumnNameInTable(nestedPrefix, property);
//
//       switch (entry.State)
//       {
//         case EntityState.Added:
//           OldValues[propertyName] = null;
//           NewValues[propertyName] = property.CurrentValue;
//           break;
//         case EntityState.Deleted:
//           OldValues[propertyName] = property.OriginalValue;
//           NewValues[propertyName] = null;
//           break;
//
//         case EntityState.Modified:
//           if (property.IsModified)
//           {
//             OldValues[propertyName] = property.OriginalValue;
//             NewValues[propertyName] = property.CurrentValue;
//           }
//
//           break;
//         case EntityState.Unchanged:
//         case EntityState.Detached:
//         default:
//           break;
//       }
//     }
//
// // https://stackoverflow.com/questions/77052913/ef-core-detect-child-changes-on-parent-at-savechanges
//     // foreach (var navigationEntry in entry.Navigations)
//     // {
//     //   switch (navigationEntry)
//     //   {
//     //     case CollectionEntry collectionEntry:
//     //     {
//     //       var i = 0;
//     //       if (collectionEntry.CurrentValue != null)
//     //         foreach (var referencedEntity in collectionEntry.CurrentValue)
//     //         {
//     //           var name = $"{nestedPrefix}{navigationEntry.Metadata.Name}[{i}].";
//     //           var childEntry = entry.Context.Entry(referencedEntity);
//     //           checkedEntries.Add(referencedEntity);
//     //           CheckAllAuditWithNestedProperties(name, childEntry, auditConfiguration);
//     //           i++;
//     //         }
//     //
//     //       break;
//     //     }
//     //     case ReferenceEntry referenceEntry:
//     //     {
//     //       var name = $"{nestedPrefix}{referenceEntry.Metadata.Name}.";
//     //       dynamic referencedEntry = referenceEntry.TargetEntry; //entry.Context.Entry(referenceEntry.TargetEntry.Entity);
//     //       if (referencedEntry != null)
//     //       {
//     //         if (!checkedEntries.Any(x => x.Entity.GetType() == referencedEntry.Entity.GetType() && x.Entity.Id == referencedEntry.Entity.Id))
//     //         {
//     //           CheckAllAuditWithNestedProperties(name, referencedEntry, auditConfiguration);
//     //         }
//     //         else
//     //         {
//     //           object entityName = referencedEntry.Entity.GetType();
//     //           _logger.LogError("{audit}:Don't use looping data types. Check entity '{entityName}' property name '{name}'.", nameof(AuditEntryItem), entityName.ToString(), name);
//     //         }
//     //       }
//     //
//     //       break;
//     //     }
//     //   }
//     // }
//   }
//
//   private string? GetTableName(IEntityType dbEntityType)
//   {
//     var tableName = dbEntityType.GetTableName();
//     if (_storageTypeDefinition.DataAnnotationTableNameKey == null)
//       return tableName;
//
//     var anno = dbEntityType.GetAnnotation(_storageTypeDefinition.DataAnnotationTableNameKey).Value?.ToString();
//     if (anno != null)
//       tableName = anno;
//
//     return tableName;
//   }
// }
//
// public static class ee
// {
//   public static long? PrimaryKeyValue(this EntityEntry entry)
//   {
//     var primaryKey = entry.Metadata.FindPrimaryKey();
//
//     var firstProp = primaryKey?.Properties.FirstOrDefault();
//     if (firstProp == null || firstProp.PropertyInfo == null)
//       return null;
//
//     var rr = firstProp.PropertyInfo.GetValue(entry.Entity);
//     if (rr == null)
//       return null;
//
//     return long.TryParse(rr.ToString(), out var pkValue) ? pkValue : null;
//   }
//
//   public static string? PrimaryKeyValueString(this EntityEntry entry)
//   {
//     var primaryKey = entry.Metadata.FindPrimaryKey();
//
//     var firstProp = primaryKey?.Properties.FirstOrDefault();
//     if (firstProp == null || firstProp.PropertyInfo == null)
//       return null;
//
//     var rr = firstProp.PropertyInfo.GetValue(entry.Entity);
//
//     return rr?.ToString();
//   }
// }