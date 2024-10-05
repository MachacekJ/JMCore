using System.Globalization;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.Mongo.Models;
using ACore.Server.Modules.AuditModule.Storage.SQL.Models;
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

internal class AuditMongoStorageImpl(DbContextOptions<AuditMongoStorageImpl> options, IMediator mediator, ILogger<AuditMongoStorageImpl> logger)
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
      Columns = saveInfoItem.ChangedColumns.Where(e => e.IsAuditable).Select(e => new AuditMongoValueEntity
      {
        PropName = e.PropName,
        Property = e.ColumnName,
        DataType = e.DataType,
        IsChanged = e.IsChanged,
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
      byte or short or int or long or bool or Guid or ObjectId => value.ToString(),
      TimeSpan ts => ts.Ticks.ToString(),
      DateTime dateTime => dateTime.Ticks.ToString(),
      decimal dec => dec.ToString(CultureInfo.InvariantCulture),
      _ => SqlConvertedItem.ToValueString(logger, value)
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
          aa.AddColumnEntry(new AuditInfoColumnItem(col.PropName, col.Property, col.DataType, col.IsChanged, SqlConvertedItem.ConvertObjectToDataType(col.DataType, col.OldValue), SqlConvertedItem.ConvertObjectToDataType(col.DataType, col.NewValue)));
        }
      }

      ll.Add(aa);
    }

    return ll.ToArray();
  }


  private static string GetObjectId(string collection, ObjectId pk) => $"{collection}.{pk}";

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditMongoEntity>().ToCollection(DefaultNames.AuditCollectionName);
  }
}