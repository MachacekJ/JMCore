// using System.Reflection;
// using ACore.Server.Configuration.Storage.Models;
// using ACore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.Mongo.TestStorageModule;
// using ACore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.Mongo.TestStorageModule.Models;
// using Xunit;
//
// namespace ACore.TestsIntegrations.ServerT.StoragesT.ModulesT.AuditStorageT;
//
// public class AuditChildValueT : AuditStructureBaseT
// {
//   protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;
//
//   //[Fact]
//   public async Task Change()
//   {
//     var method = MethodBase.GetCurrentMethod();
//     await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
//     {
//       //var auDb = GetAuditStorageModule(storageType);
//       var testDb = GetTestStorageModule(storageType) as TestMongoStorageImpl ?? throw new Exception("bbb");
//
//
//       var p = new TestParentMongoEntity()
//       {
//         Child = new TestChildMongoEntity()
//         {
//           Name1 = "Child0",
//           SubChild = new TestChildMongoEntity2
//           {
//             Name2 = "Child1",
//           Parent = new PTT() { Name2 = "dsds22" },
//            Children = new List<TT>() { new() { Name2 = "dsds" } }
//           }
//         },
//
//         // ArrayChild = new List<TestChildMongoEntity>()
//         // {
//         //   new()
//         //   {
//         //     Name1 = "ChildA0",
//         //     SubChild = new TestChildMongoEntity2
//         //     {
//         //       Name2 = "ChildA0.1",
//         //       //SubChildArr = new List<TestChildMongoEntity2>
//         //       // {
//         //       //   new ()
//         //       //   {
//         //       //     ChildName = "Child0[0]"
//         //       //   }
//         //       // }
//         //     }
//         //   }
//         // }
//       };
//
//       await testDb.TestParents.AddAsync(p);
//       await testDb.SaveChangesAsync();
//
//
//    //    p.Child.SubChild.Parent.Name2 = "dfsdfhty";
//       testDb.TestParents.Update(p);
//       // testDb.Entry(p);
//       await testDb.SaveChangesAsync();
//     });
//   }
// }