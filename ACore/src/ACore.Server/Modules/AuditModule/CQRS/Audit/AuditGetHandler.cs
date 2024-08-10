using ACore.Server.Modules.AuditModule.CQRS.Models;
using ACore.Server.Modules.AuditModule.Storage.Models;
using ACore.Server.Storages;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit;

public class AuditGetHandler<T>(IStorageResolver storageResolver) : AuditModuleRequestHandler<AuditGetQuery<T>, AuditValueData[]>(storageResolver)
{
  public override async Task<AuditValueData[]> Handle(AuditGetQuery<T> request, CancellationToken cancellationToken)
  {
    if (request.PKValue == null)
      throw new ArgumentNullException($"{nameof(request.PKValue)} is null.");


    IEnumerable<AuditValueEntity>? aa;
    if (request.PKValue != null)
      return await ReadAuditContexts().AuditItemsAsync(request.TableName, request.PKValue, request.SchemaName);

    throw new Exception($"Primary key is not found. TableName: {request.TableName}; Schema: {request.SchemaName ?? string.Empty}");
  }
}