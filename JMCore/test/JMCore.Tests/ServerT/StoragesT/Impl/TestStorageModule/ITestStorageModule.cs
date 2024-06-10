using JMCore.Server.Storages.Base.EF;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;

public interface ITestStorageModule : IDbContextBase
{
  Task AddAsync(TestValueTypeEntity item);
  Task AddAsync(TestEntity item);
  Task AddAsync(TestAttributeAuditEntity item);
  Task<IEnumerable<TestEntity>> AllTest();
  Task<IEnumerable<TestAttributeAuditEntity>> AllTestAttribute();
}