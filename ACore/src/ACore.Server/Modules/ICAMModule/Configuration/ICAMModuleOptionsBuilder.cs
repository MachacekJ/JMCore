using ACore.Base.CQRS.Configuration;

namespace ACore.Server.Modules.ICAMModule.Configuration;

public class ICAMModuleOptionsBuilder: ModuleOptionsBuilder
{
  public static ICAMModuleOptionsBuilder Empty() => new();
  public ICAMModuleOptions Build()
  {
    return new ICAMModuleOptions(IsActive);
  }
}