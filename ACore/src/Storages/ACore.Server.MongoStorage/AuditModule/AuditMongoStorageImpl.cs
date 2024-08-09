using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.Storage.Models;
using ACore.Server.MongoStorage.AuditModule.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.Server.MongoStorage.AuditModule;

public class AuditMongoStorageImpl(DbContextOptions<AuditMongoStorageImpl> options, IMediator mediator, ILogger<AuditMongoStorageImpl> logger) : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);
  protected override string ModuleName => nameof(IAuditStorageModule);
  
  public DbSet<AuditMongoEntity> Audits { get; set; }

  public async Task SaveAuditAsync(AuditEntryItem auditEntryItem)
  {
    if (!auditEntryItem.ChangedColumns.Any())
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
      Columns =auditEntryItem.ChangedColumns.Select(e=>new AuditMongoValueEntity()
      {
        Property = e.ColumnName,
        NewValue = Newtonsoft.Json.JsonConvert.SerializeObject(e.NewValue),
        OldValue = Newtonsoft.Json.JsonConvert.SerializeObject(e.OldValue), 
      }).ToList()
    };
    
    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  public Task<IEnumerable<AuditValueEntity>> AuditItemsAsync(string tableName, long pkValue, string? schemaName = null)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<AuditValueEntity>> AuditItemsAsync(string tableName, string pkValue, string? schemaName = null)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<AuditValueEntity>> AllTableAuditAsync(string tableName, string? schemaName = null)
  {
    throw new NotImplementedException();
  }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditMongoEntity>().ToCollection(CollectionNames.AuditCollectionName);
  }
}