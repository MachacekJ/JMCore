using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Server.Modules.ICAMModule.Models;

namespace ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;

public class ICAMGetCurrentUserQuery : ICAMModuleRequest<Result<UserData>>;
  