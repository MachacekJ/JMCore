using ACore.Base.CQRS.Models.Results;
using ACore.Server.Configuration;
using ACore.Server.Modules.ICAMModule.Models;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.ICAMModule.CQRS.ICAMGetCurrentUser;

public class ICAMGetCurrentUserHandler(IOptions<ACoreServerOptions> options) : ICAMModuleRequestHandler<ICAMGetCurrentUserQuery, Result<UserData>>
{
  public override Task<Result<UserData>> Handle(ICAMGetCurrentUserQuery request, CancellationToken cancellationToken)
  {
    return Task.FromResult(Result.Success(new UserData(UserTypeEnum.Test, "1", "testUser")));
    // return request.UserType switch
    // {
    //   UserTypeEnum.Test => Task.FromResult(Result.Success(new UserData(request.UserType, "1", "testUser"))),
    //   UserTypeEnum.System=> Task.FromResult(Result.Success(new UserData(request.UserType, "-1", "system"))),
    //   _ => Task.FromResult(Result.Failure<UserData>(new Error("CredentialModule", $"Unknown {nameof(request.UserType)} ")))
    // };
  }
}