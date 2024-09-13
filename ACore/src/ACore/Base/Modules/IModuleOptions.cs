namespace ACore.Base.Modules;

public interface IModuleOptions
{
  public bool IsActive { get; }
  public string ModuleName { get; }
}