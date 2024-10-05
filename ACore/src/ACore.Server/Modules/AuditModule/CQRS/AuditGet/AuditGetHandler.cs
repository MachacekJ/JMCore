using ACore.Base.CQRS.Results;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Storages.Services.StorageResolvers;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet;

public class AuditGetHandler<TPK>(IStorageResolver storageResolver) : AuditModuleRequestHandler<AuditGetQuery<TPK>, Result<AuditGetQueryDataOut<TPK>[]>>(storageResolver)
{
  public override async Task<Result<AuditGetQueryDataOut<TPK>[]>> Handle(AuditGetQuery<TPK> request, CancellationToken cancellationToken)
  {
    if (request.PKValue == null)
      throw new Exception($"Primary key is not found. TableName: {request.TableName}; Schema: {request.SchemaName ?? string.Empty}");

    var r = (await ReadAuditContext().AuditItemsAsync(request.TableName, request.PKValue, request.SchemaName))
      .Select(AuditGetQueryDataOut<TPK>.Create).ToArray();

    return Result.Success(r);
  }
}