﻿using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet.Models;
using ACore.Server.Storages;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet;

public class AuditGetHandler<T>(IStorageResolver storageResolver) : AuditModuleRequestHandler<AuditGetQuery<T>, AuditGetQueryDataOut[]>(storageResolver)
{
  public override async Task<AuditGetQueryDataOut[]> Handle(AuditGetQuery<T> request, CancellationToken cancellationToken)
  {
    if (request.PKValue == null)
      throw new Exception($"Primary key is not found. TableName: {request.TableName}; Schema: {request.SchemaName ?? string.Empty}");

    return (await ReadAuditContexts().AuditItemsAsync(request.TableName, request.PKValue, request.SchemaName))
      .Select(AuditGetQueryDataOut.Create).ToArray();
  }
}