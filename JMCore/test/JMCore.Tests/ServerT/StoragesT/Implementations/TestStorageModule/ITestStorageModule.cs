using JMCore.Server.Storages.Base.EF;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;

namespace JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;

public interface ITestStorageModule
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