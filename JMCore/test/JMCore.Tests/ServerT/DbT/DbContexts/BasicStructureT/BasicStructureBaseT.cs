﻿using JMCore.Server.DB.DbContexts.BasicStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.DbT.DbContexts.BasicStructureT;

public class BasicStructureBaseT : DbBaseT
{
    protected BasicDbContext Db = null!;


    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddDbContext<BasicDbContext>(opt => opt.UseInMemoryDatabase(nameof(BasicDbContext) + TestData.TestName));
        sc.AddScoped<IBasicDbContext, BasicDbContext>();
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        Db = sp.GetService<BasicDbContext>() ?? throw new ArgumentException($"{nameof(BasicDbContext)} is null.");
    }
}