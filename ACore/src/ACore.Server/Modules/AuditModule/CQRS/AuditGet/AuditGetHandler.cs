using ACore.Base.CQRS.Models.Results;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Storages;
using ACore.Server.Storages.Definitions.Models.PK;
using ACore.Server.Storages.Services.StorageResolvers;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet;

public class AuditGetHandler<TEntity, TPK>(IStorageResolver storageResolver) : AuditModuleRequestHandler<AuditGetQuery<TEntity, TPK>, Result<AuditGetQueryDataOut<TEntity, TPK>[]>>(storageResolver)
  where TEntity : PKEntity<TPK>
{
  public override async Task<Result<AuditGetQueryDataOut<TEntity, TPK>[]>> Handle(AuditGetQuery<TEntity, TPK> request, CancellationToken cancellationToken)
  {
    if (request.PKValue == null)
      throw new Exception($"Primary key is not found. TableName: {request.TableName}; Schema: {request.SchemaName ?? string.Empty}");

    var r = (await ReadAuditContext().AuditItemsAsync(request.TableName, request.PKValue, request.SchemaName))
      .Select(AuditGetQueryDataOut<TEntity, TPK>.Create).ToArray();

    return Result.Success(r);
  }
}