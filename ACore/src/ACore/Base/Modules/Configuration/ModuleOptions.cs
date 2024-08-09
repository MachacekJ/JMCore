namespace ACore.Base.Modules.Configuration;

public abstract class ModuleOptions(string moduleName, bool isActive) : IModuleOptions
{
  public string ModuleName => moduleName;
  public bool IsActive => isActive;
}