// using System.Reflection;
// using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
// using Xunit;
//
// namespace ACore.TestsIntegrations.Modules.TestModule.MongoT;
//
// public class ParentChildTests : TestStorageModuleBaseTests
// {
//   [Fact]
//   public async Task ParentChildHierarchyTest()
//   {
//     var method = MethodBase.GetCurrentMethod();
//     await RunStorageTestAsync(StorageTypesToTest, method, async (_) =>
//     {
//       var testDb = GetTestStorageImplementation();
//
//       var parent = new TestRootCategory
//       {
//         Name = "Ahoj",
//         LastModify = DateTime.UtcNow,
//         SubCategories = new List<TestCategory>
//         {
//           new()
//           {
//             Name = "aa"
//           }
//         }
//       };
//
//       await testDb.TestParents.AddAsync(parent);
//       await testDb.SaveChangesAsync();
//
//       var pc = parent.SubCategories.First();
//       pc.SubCategories = new List<TestCategory>() { new() { Name = "vv", RootCategoryId = pc.Id } };
//       await testDb.SaveChangesAsync();
//     });
//   }
// }