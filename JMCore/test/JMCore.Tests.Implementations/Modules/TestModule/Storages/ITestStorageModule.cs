using JMCore.Server.Storages;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages;

public interface ITestStorageModule : IStorage
{
  Task AddAsync(TestPKGuidEntity item);
  Task AddAsync(TestPKStringEntity item);
  Task AddAsync(TestValueTypeEntity item);
  Task AddAsync(TestEntity item);
  Task AddAsync(TestAttributeAuditEntity item);
  Task AddAsync(TestManualAuditEntity item);
  Task UpdateAsync(TestAttributeAuditEntity item);
  Task UpdateAsync(TestManualAuditEntity item);
  Task UpdateAsync(TestPKGuidEntity item);
  Task UpdateAsync(TestPKStringEntity item);
  Task DeleteAsync(TestAttributeAuditEntity item);
  Task DeleteAsync(TestManualAuditEntity item);
  Task<IEnumerable<TestEntity>> AllTest();
  Task<IEnumerable<TestAttributeAuditEntity>> AllTestAttribute();
  Task<IEnumerable<TestManualAuditEntity>> AllTestManual();
  Task<IEnumerable<TestPKGuidEntity>> AllTestPKGuid();
  Task<IEnumerable<TestPKStringEntity>> AllTestPKString();
}