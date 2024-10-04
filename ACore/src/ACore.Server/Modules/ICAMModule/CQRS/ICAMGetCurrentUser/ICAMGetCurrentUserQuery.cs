using ACore.Base.CQRS.Results;
using ACore.Server.Modules.ICAMModule.Models;

namespace ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;

public class ICAMGetCurrentUserQuery : ICAMModuleRequest<Result<UserData>>;
  