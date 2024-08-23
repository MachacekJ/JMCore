using ACore.Server.Modules.AuditModule.Storage.SQL.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Storage.SQL.PG;

internal class AuditPGEfStorageImpl(DbContextOptions<AuditPGEfStorageImpl> options, IMediator mediator, ILogger<AuditSqlStorageImpl> logger) : AuditSqlStorageImpl(options, mediator, logger)
{
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditColumnEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditTableEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditUserEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditValueEntity>().HasKey(p => p.Id);
    
    SetDatabaseNames<AuditColumnEntity>(modelBuilder);
    SetDatabaseNames<AuditEntity>(modelBuilder);
    SetDatabaseNames<AuditTableEntity>(modelBuilder);
    SetDatabaseNames<AuditUserEntity>(modelBuilder);
    SetDatabaseNames<AuditValueEntity>(modelBuilder);
  }

  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(AuditPGEfDbNames.ObjectNameMapping, modelBuilder);
}