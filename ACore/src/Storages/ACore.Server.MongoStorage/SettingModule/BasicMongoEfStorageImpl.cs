using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Modules.SettingModule.Storage.BaseImpl;
using ACore.Server.Modules.SettingModule.Storage.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.Server.MongoStorage.SettingModule;

public class BasicSqlMongoEfStorageImpl : BasicSqlStorageImpl
{
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);

  public BasicSqlMongoEfStorageImpl(DbContextOptions<BasicSqlMongoEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlMongoEfStorageImpl> logger) : base(options, mediator, logger)
  {
  }

  public BasicSqlMongoEfStorageImpl(DbContextOptions<BasicSqlMongoEfStorageImpl> options, IMediator mediator, IAuditDbService? auditService, IAuditConfiguration? auditConfiguration, ILogger<BasicSqlMongoEfStorageImpl> logger) : base(options, mediator, auditService, auditConfiguration, logger)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SettingEntity>().Ignore(t => t.Id);
  
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingEntity>().ToCollection(CollectionNames.SettingsCollectionName);
    modelBuilder.Entity<SettingEntity>(builder => 
      builder.Property(entity => entity.Key).HasElementName("_id")
    );
    modelBuilder.Entity<SettingEntity>().HasKey(p => p.Key);
  }
}