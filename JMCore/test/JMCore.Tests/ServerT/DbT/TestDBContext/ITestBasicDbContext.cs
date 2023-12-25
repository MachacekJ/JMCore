using JMCore.Server.DB.Abstract;
using JMCore.Tests.ServerT.DbT.TestDBContext.Models;

namespace JMCore.Tests.ServerT.DbT.TestDBContext;

public interface ITestBasicDbContext : IDbContextBase
{
    Task<int> Test_AddAsync(TestEntity data);
    Task<Dictionary<int, TestEntity>> Test_AddAsync(List<TestEntity> data);

    Task<TestEntity> Test_GetAsync(int id);

    Task<int> Test_SaveAsync(TestEntity data);
}