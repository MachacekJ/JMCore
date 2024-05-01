using JMCore.Server.Storages.Base.EF;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;

public interface ITestStorageModule : IDbContextBase
{
    Task<int> Test_AddAsync(TestEntity data);
    Task<Dictionary<int, TestEntity>> Test_AddAsync(List<TestEntity> data);

    Task<TestEntity> Test_GetAsync(int id);

    Task<int> Test_SaveAsync(TestEntity data);
}