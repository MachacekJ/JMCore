using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Models.SaveInfo;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveCommand(SaveInfoItem saveInfoItem) : AuditModuleRequest<Result>
{
  public SaveInfoItem SaveInfoItem => saveInfoItem;
}