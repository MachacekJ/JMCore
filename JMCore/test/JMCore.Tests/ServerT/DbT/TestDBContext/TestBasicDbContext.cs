using JMCore.Server.Storages.Abstract;
using JMCore.Server.Storages.Audit;
using JMCore.Tests.ServerT.DbT.TestDBContext.Models;
using JMCore.Tests.ServerT.DbT.TestDBContext.Scripts;
using JMCore.Tests.ServerT.DbT.TestDBContext.Scripts.Postgres;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext;

public class TestBasicDbContext : DbContextBase, ITestBasicDbContext
{
    public DbSet<TestEntity> Tests { get; set; } = null!;

    public DbSet<TestManualAuditEntity> TestManualAudits { get; set; }

    public DbSet<TestAttributeAuditEntity> TestAttributeAudits { get; set; }
    public DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
    public DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
    public DbSet<TestPKStringEntity> TestPKString { get; set; }

    public override DbScriptBase SqlScripts => new ScriptRegistrations();
    public override string DbContextName => GetType().Name;

    public TestBasicDbContext(DbContextOptions<TestBasicDbContext> options, IMediator mediator, ILogger<TestBasicDbContext> logger, IAuditDbService auditService) : base(options, mediator, logger, auditService)
    {
    }

    public async Task<int> Test_AddAsync(TestEntity data)
    {
        return (await Test_AddAsync(new List<TestEntity>() { data }))[0].Id;
    }

    public async Task<Dictionary<int, TestEntity>> Test_AddAsync(List<TestEntity> data)
    {
        var res = new Dictionary<int, TestEntity>();

        foreach (var item in data)
        {
            item.Created = item.Created.ToUniversalTime();
            Tests.Add(item);
        }

        await SaveChangesAsync();

        foreach (var item in data)
        {
            res.Add(item.Id, item);
        }

        return res;
    }

    #region Test Table

    public async Task<TestEntity> Test_GetAsync(int id)
    {
        TestEntity res = await Tests.Where(r => r.Id == id).FirstAsync();
        return res;
    }

    public async Task<int> Test_SaveAsync(TestEntity data)
    {
        Tests.Add(data);
        await SaveChangesAsync();
        return data.Id;
    }

    #endregion
}