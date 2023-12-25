using System.Reflection;
using FluentAssertions;
using JMCore.Tests.ServerT.DbT.TestDBContext.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

// ReSharper disable InconsistentNaming

namespace JMCore.Tests.ServerT.DbT.DbContexts.AuditStructureT;

public class AuditPKT : AuditAttributeBaseT
{
    const string testName = "AuditPK";
    private const string tableGuidName = nameof(TestPKGuidEntity);
    private const string tableStringName = nameof(TestPKStringEntity);
    
    [Fact]
    public async Task GuidPK()
    {
        var Id = Guid.NewGuid();

        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange.
            var item = new TestPKGuidEntity()
            {
                Id = Id,
                Name = testName,
            };
            
            // Action 1
            await TestBasicDbContext.TestPKGuid.AddAsync(item);
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 1
            Assert.True(await TestBasicDbContext.TestPKGuid.CountAsync() == 1);
            var auditValues = await AuditDbContext.VwAudits().Where(a => a.TableName == tableGuidName && a.PKValueString == item.Id.ToString()).ToListAsync();
            auditValues.Count.Should().Be(2);
            auditValues.Single(a => a.NewValueGuid == item.Id).NewValueGuid.Should().Be(item.Id);

            // Arrange 2
            item.Name = testName + "2";
            
            // Action 2 
            await TestBasicDbContext.SaveChangesAsync();
            
            // Assert 2
            auditValues = await AuditDbContext.VwAudits().Where(a => a.TableName == tableGuidName && a.PKValueString == item.Id.ToString()).ToListAsync();
            auditValues.Count(a=>a.PKValueString == item.Id.ToString()).Should().Be(3);
            
        });
    }
    
    [Fact]
    public async Task StringPK()
    {
        var Id = Guid.NewGuid().ToString() + "ř Ř ě";

        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange.
            var item = new TestPKStringEntity()
            {
                Id = Id,
                Name = testName,
            };
            
            // Action 1
            await TestBasicDbContext.TestPKString.AddAsync(item);
            await TestBasicDbContext.SaveChangesAsync();

            // Assert 1
            Assert.True(await TestBasicDbContext.TestPKString.CountAsync() == 1);
            var auditValues = await AuditDbContext.VwAudits().Where(a => a.TableName == tableStringName && a.PKValueString == item.Id).ToListAsync();
            auditValues.Count.Should().Be(2);
            auditValues.Single(a => a.NewValueString == item.Id).NewValueString.Should().Be(item.Id);

            // Arrange 2
            item.Name = testName + "2";
            
            // Action 2 
            await TestBasicDbContext.SaveChangesAsync();
            
            // Assert 2
            auditValues = await AuditDbContext.VwAudits().Where(a => a.TableName == tableStringName && a.PKValueString == item.Id).ToListAsync();
            auditValues.Count(a=>a.PKValueString == item.Id).Should().Be(3);
        });
    }
}