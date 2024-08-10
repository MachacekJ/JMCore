using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.CQRS.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.MongoStorage.AuditModule.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
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


    var auditEntity = new AuditMongoEntity
    {
      ObjectId = GetObjectId(auditEntryItem.TableName, new ObjectId(auditEntryItem.PkValueString)),
      User = new AuditMongoUserEntity
      {
        Id = auditEntryItem.ByUser.userId,
        Name = auditEntryItem.ByUser.userName
      },
      EntityState = auditEntryItem.EntityState,
      Time = DateTime.UtcNow,
      Columns = auditEntryItem.ChangedColumns.Select(e => new AuditMongoValueEntity()
      {
        Property = e.ColumnName,
        DataType = e.DataType.FullName ?? string.Empty,
        NewValue = e.NewValue?.ToString(),
        OldValue = e.OldValue?.ToString(),
      }).ToList()
    };

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  public async Task<AuditValueData[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null)
  {
    var valuesTable = await Audits.Where(e => e.ObjectId == GetObjectId(tableName, new ObjectId(pkValue.ToString()))).ToArrayAsync();
    var ll = new List<AuditValueData>();
    foreach (var imte in valuesTable)
    {
      var aa = new AuditValueData
      {
        TableName = tableName,
        EntityState = imte.EntityState.ToAuditStateEnum(),
        DateTime = imte.Time,
        PKValueString = pkValue.ToString(),
        UserName = imte.User.Name
      };

      var ccc = new List<AuditValueColumnData>();

      foreach (var col in imte.Columns)
      {
        var coltype = col.DataType ?? throw new Exception($"Cannot create data type '{col.DataType}' from {nameof(AuditMongoValueEntity)}:'{imte._id}'");
        var bb = new AuditValueColumnData
        (
          col.Property,
          coltype,
          CC(col.OldValue, coltype),
          CC(col.NewValue, coltype)
        );
        ccc.Add(bb);
      }

      aa.Columns = ccc.ToArray();
      ll.Add(aa);
    }

    return ll.ToArray();
  }

  private object? CC(string? value, string dataType)
  {
    if (string.IsNullOrEmpty(dataType))
      throw new ArgumentNullException($"Data type is null.");

    if (value == null || value == "null")
      return null;
    
    if (dataType == typeof(ObjectId).FullName)
      return new ObjectId(value);
    
    var type = Type.GetType(dataType);
    if (type == null)
      throw new Exception($"Cannot create data type '{dataType}'.");


    
    
    var c = Convert.ChangeType(value, type);
    return c;
  }

  private static string GetObjectId(string collection, ObjectId pk) => $"{collection}.{pk}";

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditMongoEntity>().ToCollection(CollectionNames.AuditCollectionName);
  }
}