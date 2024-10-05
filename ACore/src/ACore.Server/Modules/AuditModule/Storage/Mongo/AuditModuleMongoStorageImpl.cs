using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.Mongo.Models;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.Base;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.EF.MongoStorage;
using ACore.Server.Storages.Models.SaveInfo;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;


namespace ACore.Server.Modules.AuditModule.Storage.Mongo;

internal class AuditModuleMongoStorageImpl(DbContextOptions<AuditModuleMongoStorageImpl> options, IMediator mediator, ILogger<AuditModuleMongoStorageImpl> logger)
  : DbContextBase(options, mediator, logger), IAuditStorageModule
{
  protected override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();
  protected override string ModuleName => nameof(IAuditStorageModule);

  // ReSharper disable once UnusedAutoPropertyAccessor.Global
  public DbSet<AuditMongoEntity> Audits { get; set; }

  public async Task SaveAuditAsync(SaveInfoItem saveInfoItem)
  {
    if (saveInfoItem.IsAuditable == false || !saveInfoItem.ChangedColumns.Any())
      return;
    
    var auditEntity = new AuditMongoEntity
    {
      ObjectId = GetObjectId(saveInfoItem.TableName, new ObjectId(saveInfoItem.PkValueString)),
      Version = saveInfoItem.Version,
      User = new AuditMongoUserEntity
      {
        Id = saveInfoItem.UserId
      },
      EntityState = saveInfoItem.EntityState,
      Created = DateTime.UtcNow,
      Columns = saveInfoItem.ChangedColumns.Where(e=>e.IsAuditable).Select(e => new AuditMongoValueEntity
      {
        PropName = e.PropName,
        Property = e.ColumnName,
        DataType = e.DataType,
        NewValue = ConvertValue(e.NewValue),
        OldValue = ConvertValue(e.OldValue),
      }).ToList()
    };

    await Audits.AddAsync(auditEntity);
    await SaveChangesAsync();
  }

  private string? ConvertValue(object? value)
  {
    if (value == null)
      return null;

    return value switch
    {
      DateTime dateTime => dateTime.Ticks.ToString(),
      _ => value.ToString()
    };
  }

  public async Task<AuditInfoItem[]> AuditItemsAsync<TPK>(string collectionName, TPK pkValue, string? schemaName = null)
  {
    if (pkValue == null)
      throw new Exception("Primary key is null");

    var valuesTable = await Audits.Where(e => e.ObjectId == GetObjectId(collectionName, new ObjectId(pkValue.ToString()))).ToArrayAsync();
    var ll = new List<AuditInfoItem>();
    foreach (var auditMongoEntity in valuesTable)
    {
      var aa = new AuditInfoItem(collectionName, null, auditMongoEntity.Version, pkValue, auditMongoEntity.EntityState, auditMongoEntity.User.Id);
      aa.Created = auditMongoEntity.Created;

      if (auditMongoEntity.Columns != null)
      {
        foreach (var col in auditMongoEntity.Columns)
        {
          var coltype = col.DataType ?? throw new Exception($"Cannot create data type '{col.DataType}' from {nameof(AuditMongoValueEntity)}:'{auditMongoEntity.Id}'");
          aa.AddColumnEntry(new AuditInfoColumnItem(col.PropName, col.Property, col.DataType, col.IsChanged, ConvertToObject(col.OldValue, coltype), ConvertToObject(col.NewValue, coltype)));
        }
      }

      ll.Add(aa);
    }

    return ll.ToArray();
  }

  private object? ConvertToObject(string? value, string dataType)
  {
    if (string.IsNullOrEmpty(dataType))
      throw new ArgumentNullException($"Data type is null.");

    if (value == null || value == "null")
      return null;

    if (dataType == typeof(ObjectId).FullName)
      return new ObjectId(value);

    if (dataType == typeof(DateTime).FullName)
      return new DateTime(Convert.ToInt64(value), DateTimeKind.Utc);

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
    modelBuilder.Entity<AuditMongoEntity>().ToCollection(DefaultNames.AuditCollectionName);
  }
}