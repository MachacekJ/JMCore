using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.Storages;

internal interface ITestStorageModule : IStorage
{
  Task SaveAsync(TestPKGuidEntity item);
  Task SaveAsync(TestPKStringEntity item);
  Task SaveAsync(TestValueTypeEntity item);
  Task SaveAsync(TestEntity item);
  Task SaveAsync(TestAttributeAuditEntity item);
  
  // Task AddAsync(TestPKGuidEntity item);
  // Task AddAsync(TestPKStringEntity item);
  // Task AddAsync(TestValueTypeEntity item);
  // Task AddAsync(TestEntity item);
  // Task AddAsync(TestAttributeAuditEntity item);
  Task SaveAsync(TestManualAuditEntity item);
 // Task UpdateAsync(TestAttributeAuditEntity item);
 // Task UpdateAsync(TestManualAuditEntity item);
//  Task UpdateAsync(TestPKGuidEntity item);
//  Task UpdateAsync(TestPKStringEntity item);
  Task DeleteAsync(TestAttributeAuditEntity item);
  Task DeleteAsync(TestManualAuditEntity item);
  Task<IEnumerable<TestEntity>> AllTest();
  Task<IEnumerable<TestAttributeAuditEntity>> AllTestAttribute();
  Task<IEnumerable<TestManualAuditEntity>> AllTestManual();
  Task<IEnumerable<TestPKGuidEntity>> AllTestPKGuid();
  Task<IEnumerable<TestPKStringEntity>> AllTestPKString();
  Task<IEnumerable<TestValueTypeEntity>> AllTestValueTypeString();
}