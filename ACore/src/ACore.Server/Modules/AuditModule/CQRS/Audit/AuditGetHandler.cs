using ACore.Server.Modules.AuditModule.CQRS.Models;
using ACore.Server.Modules.AuditModule.Storage.Models;
using ACore.Server.Storages;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit;

internal class AuditGetHandler(IStorageResolver storageResolver) : AuditModuleRequestHandler<AuditGetQuery, AuditValueData[]>(storageResolver)
{
  public override async Task<AuditValueData[]> Handle(AuditGetQuery request, CancellationToken cancellationToken)
  {
    if (request.PKValue == null && request.PKStringValue == null)
      throw new ArgumentNullException($"{nameof(request.PKValue)} or {nameof(request.PKStringValue)} is null.");

    var res = new List<AuditValueData>();
    var aa = await ReadAuditContexts().AllTableAuditAsync(request.TableName, request.SchemaName); // .Select(TestAttributeAuditData.Create);
    foreach (var grItem in aa.GroupBy(e =>
               new
               {
                 tableName = e.Audit.AuditTable.TableName,
                 schemaName = e.Audit.AuditTable.SchemaName,
                 pk = e.Audit.PKValue,
                 pkString = e.Audit.PKValueString,
                 entityState = e.Audit.EntityState
                 //userName= e.Audit.User.UserName
               }))
    {
      var aab = new AuditValueData
      {
        TableName = grItem.Key.tableName,
        PKValue = grItem.Key.pk,
        PKValueString = grItem.Key.pkString,
        SchemaName = grItem.Key.schemaName,
        EntityState = grItem.Key.entityState.ToAuditStateEnum()
      };

      var cc = new List<AuditValueColumnData>();
      foreach (var col in grItem.ToArray())
      {
        cc.Add(new AuditValueColumnData
        {
          ColumnName = col.AuditColumn.ColumnName,
          NewValue = col.GetNewValueObject(),
          OldValue = col.GetOldValueObject()
        });
      }

      aab.Columns = cc.ToArray();
      res.Add(aab);
    }

    return res.ToArray();
  }
}