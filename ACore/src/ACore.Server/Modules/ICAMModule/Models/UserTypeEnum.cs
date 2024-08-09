namespace ACore.Server.Modules.ICAMModule.Models;

public enum UserTypeEnum
{
  System,
  Test  
}

public static class UserTypeExtensions
{
  public static string ToCode(this UserTypeEnum userType)
  {
    return userType switch
    {
      UserTypeEnum.System => "SYS",
      UserTypeEnum.Test => "TEST",
      _ => throw new ArgumentOutOfRangeException($"Unknown '{nameof(UserTypeEnum)}.{Enum.GetName(userType)}'.")
    };
  }
}