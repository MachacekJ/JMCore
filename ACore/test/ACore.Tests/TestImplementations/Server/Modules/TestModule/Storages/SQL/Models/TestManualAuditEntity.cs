// // ReSharper disable UnusedAutoPropertyAccessor.Global
// using System.ComponentModel.DataAnnotations;
// using ACore.Extensions;
// using ACore.Server.Storages.Models.PK;
// using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Models;
//
// namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
//
// internal class TestManualAuditEntity : PKLongEntity
// {
//   [MaxLength(200)]
//   public string Name { get; set; } = string.Empty;
//   
//   [MaxLength(200)]
//   public string NotAuditableColumn { get; set; } = string.Empty;
//   
//   public DateTime Created { get; set; }
//
//   public static TestManualAuditEntity Create(TestManualAuditData data)
//   {
//     var en = new TestManualAuditEntity();
//     en.CopyPropertiesFrom(data);
//     return en;
//   }
// }
//
// internal static class TestManualAuditEntityExtensions
// {
//   public static TestManualAuditData ToData(this TestManualAuditEntity entity)
//   {
//     var data = new TestManualAuditData();
//     data.CopyPropertiesFrom(entity);
//     return data;
//   }
// }
//
