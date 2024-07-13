using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.MongoStorage.AuditModule.Models;
using JMCore.Server.Storages.Base.Audit.Models;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.AuditModule.Models;
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

  public async Task SaveAuditAsync(AuditEntry auditEntry)
  {
    var entityNameFullName = string.Empty;
    if (!string.IsNullOrEmpty(auditEntry.SchemaName))
      entityNameFullName = $"{auditEntry.SchemaName}.";

    entityNameFullName += $"{auditEntry.TableName}.";

    if (auditEntry.PkValue != null)
      entityNameFullName += $"{auditEntry.PkValue}.";

    if (auditEntry.PkValueString != null)
      entityNameFullName += $"{auditEntry.PkValueString}.";

    if (entityNameFullName.EndsWith("."))
      entityNameFullName = entityNameFullName.Substring(0, entityNameFullName.Length - 1);

    var auditEntity = new AuditMongoEntity
    {
      ObjectId = entityNameFullName,
      OldValues = auditEntry.EntityState != EntityState.Added
        ? auditEntry.OldValues.Select(a => new AuditMongoValues() { Column = a.Key, Value = Newtonsoft.Json.JsonConvert.SerializeObject(a.Value) }).ToList()
        : null,
      NewValues =  auditEntry.EntityState != EntityState.Deleted 
        ? auditEntry.NewValues.Select(a => new AuditMongoValues() { Column = a.Key, Value = Newtonsoft.Json.JsonConvert.SerializeObject(a.Value) }).ToList()
        :null,
      User = new AuditMongoUser
      {
        UserId = auditEntry.ByUser.userId,
        UserName = auditEntry.ByUser.userName
      },
      EntityState = auditEntry.EntityState,
      DateTime = DateTime.UtcNow
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
    modelBuilder.Entity<AuditMongoEntity>().HasKey(p => p.Id);
  }
}