using JMCore.Server.Modules.AuditModule.Models;
using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Modules.AuditModule.Storage.Models;
using JMCore.Server.MongoStorage.AuditModule.Models;
using JMCore.Server.Storages.EF;
using JMCore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace JMCore.Server.MongoStorage.AuditModule;

public class AuditMongoStorageImpl(DbContextOptions<AuditMongoStorageImpl> options, IMediator mediator, ILogger<AuditMongoStorageImpl> logger) : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);
  protected override string ModuleName => nameof(IAuditStorageModule);

  public DbSet<AuditMongoEntity> Audits { get; set; }

  public async Task SaveAuditAsync(AuditEntryItem auditEntryItem)
  {
    if (!auditEntryItem.OldValues.Any() && !auditEntryItem.NewValues.Any())
      return;
    
    var entityNameFullName = string.Empty;
    if (!string.IsNullOrEmpty(auditEntryItem.SchemaName))
      entityNameFullName = $"{auditEntryItem.SchemaName}.";

    entityNameFullName += $"{auditEntryItem.TableName}.";

    if (auditEntryItem.PkValue != null)
      entityNameFullName += $"{auditEntryItem.PkValue}.";

    if (auditEntryItem.PkValueString != null)
      entityNameFullName += $"{auditEntryItem.PkValueString}.";

    if (entityNameFullName.EndsWith("."))
      entityNameFullName = entityNameFullName.Substring(0, entityNameFullName.Length - 1);
    
    var auditEntity = new AuditMongoEntity
    {
      ObjectId = entityNameFullName,
      User = new AuditMongoUserEntity
      {
        Id = auditEntryItem.ByUser.userId,
        Name = auditEntryItem.ByUser.userName
      },
      EntityState = auditEntryItem.EntityState,
      Time = DateTime.UtcNow,
      OldValues = auditEntryItem.OldValues.Select(e=>new AuditMongoValueEntity()
      {
        Property = e.Key,
        Value = Newtonsoft.Json.JsonConvert.SerializeObject(e.Value)
      }).ToList(),
      NewValues = auditEntryItem.NewValues.Select(e=>new AuditMongoValueEntity()
      {
        Property = e.Key,
        Value = Newtonsoft.Json.JsonConvert.SerializeObject(e.Value)
      }).ToList(),
    };
    
    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  public Task<IEnumerable<AuditVwAuditEntity>> AuditItemsAsync(string tableName, int pkValue, string? schemaName = null)
  {
    var aa = new List<AuditVwAuditEntity> { new() { AuditId = 1 } };
    return Task.FromResult<IEnumerable<AuditVwAuditEntity>>(aa);
  }

  public Task<IEnumerable<AuditVwAuditEntity>> AuditItemsAsync(string tableName, string pkValue, string? schemaName = null)
  {
    var aa = new List<AuditVwAuditEntity> { new() { AuditId = 1 } };
    return Task.FromResult<IEnumerable<AuditVwAuditEntity>>(aa);
  }

  public Task<IEnumerable<AuditVwAuditEntity>> AllAuditItemsAsync(string tableName, string? schemaName = null)
  {
    var aa = new List<AuditVwAuditEntity> { new() { AuditId = 1 } };
    return Task.FromResult<IEnumerable<AuditVwAuditEntity>>(aa);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditMongoEntity>().ToCollection(CollectionNames.AuditCollectionName);
  }
}