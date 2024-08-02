using System.Reflection;
using JMCore.Tests.BaseInfrastructure.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.PG.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.TestStorageModuleT.PGT;

public class ParentChildTests : TestStorageModuleBaseTests
{
  [Fact]
  public async Task ParentChildAndHierarchyTest()
  {
    var menuName = "menuName";
    var categoryName = "categoryName";
    var subCategoryName = "subCategoryName";
    var method = MethodBase.GetCurrentMethod();
    var testData = new TestData(method)
    {
      DatabaseManipulation = DatabaseManipulationEnum.Create
    };
    
    await RunStorageTestAsync(StorageTypesToTest,testData,  async (_) =>
    {
      var testDb = GetTestStorageImplementation();

      var parent = new TestMenuEntity
      {
        Name = menuName,
        LastModify = DateTime.UtcNow,
        Categories = new List<TestCategoryEntity>
        {
          new()
          {
            Name = categoryName
          }
        }
      };

      await testDb.TestMenus.AddAsync(parent);
      await testDb.SaveChangesAsync();

      var pc = parent.Categories.First();
      pc.SubCategories = new List<TestCategoryEntity>() { new() { Name = subCategoryName, MenuId = pc.Id } };
      await testDb.SaveChangesAsync();
    });

    var testData2 = new TestData(method)
    {
      DatabaseManipulation = DatabaseManipulationEnum.Drop
    };
    await RunStorageTestAsync(StorageTypesToTest, testData2, async (_) =>
    {
      var testDb = GetTestStorageImplementation();
     // var pp = testDb.TestMenus.;

      
      
      var categ = await testDb.TestCategories.Where(e => e.ParentCategoryId == null).ToListAsync();
      var bb = categ[0].ParentCategory;
    });
  }
}