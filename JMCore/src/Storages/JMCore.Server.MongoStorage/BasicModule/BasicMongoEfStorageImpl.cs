using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.BasicModule.BaseImpl;
using JMCore.Server.Storages.Modules.BasicModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace JMCore.Server.MongoStorage.BasicModule;

public class BasicSqlMongoEfStorageImpl : BasicSqlStorageImpl
{
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);

  public BasicSqlMongoEfStorageImpl(DbContextOptions<BasicSqlMongoEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlMongoEfStorageImpl> logger) : base(options, mediator, logger)
  {
  }

  public BasicSqlMongoEfStorageImpl(DbContextOptions<BasicSqlMongoEfStorageImpl> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicSqlMongoEfStorageImpl> logger) : base(options, mediator, auditService, logger)
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