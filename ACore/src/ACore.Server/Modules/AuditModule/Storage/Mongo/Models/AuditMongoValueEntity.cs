using ACore.Extensions;
using Amazon.Runtime.Internal.Transform;
using MongoDB.Bson.Serialization.Attributes;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Storage.Mongo.Models;

internal class AuditMongoValueEntity
{
  // property name
  [BsonElement("pn")]
  public string PropName { get; set; }
  
  // name
  [BsonElement("n")]
  public string Property { get; set; }
  
  // type
  [BsonElement("t")]
  public string DataType { get; set; }
  
  // changed
  [BsonElement("ch")]
  public bool IsChanged { get; set; }
  
  [BsonElement("ov")]
  public string? OldValue { get; set; }
  [BsonElement("nv")]
  public string? NewValue { get; set; }
}

// internal static class AuditMongoValueEntityExtensions
// {
//   public static object? GetNewValueObject(this AuditMongoValueEntity auditSqlValueItem)
//   {
//     if (auditSqlValueItem.NewValue == null)
//       return null;
//     
//     if (auditSqlValueItem.DataType == typeof(int).ACoreTypeName())
//       return 
//     
//     switch (auditSqlValueItem.DataType)
//     {
//       case typeof(DateTime).ACoreTypeName():
//         break;
//     }
//     
//     if (auditSqlValueItem.NewValueInt != null)
//       return auditSqlValueItem.NewValueInt;
//     if (auditSqlValueItem.NewValueLong != null)
//       return auditSqlValueItem.NewValueLong;
//     if (auditSqlValueItem.NewValueString != null)
//       return auditSqlValueItem.NewValueString;
//     if (auditSqlValueItem.NewValueBool != null)
//       return auditSqlValueItem.NewValueBool;
//     return auditSqlValueItem.NewValueGuid ?? null;
//   }
//
//   public static object? GetOldValueObject(this AuditMongoValueEntity auditSqlValueItem)
//   {
//     if (auditSqlValueItem.OldValueInt != null)
//       return auditSqlValueItem.OldValueInt;
//     if (auditSqlValueItem.OldValueLong != null)
//       return auditSqlValueItem.OldValueLong;
//     if (auditSqlValueItem.OldValueString != null)
//       return auditSqlValueItem.OldValueString;
//     if (auditSqlValueItem.OldValueBool != null)
//       return auditSqlValueItem.OldValueBool;
//     return auditSqlValueItem.OldValueGuid ?? null;
//   }
// }