namespace ACore.Configuration;

public interface IModuleOptions
{
  public bool IsActive { get; }
  public string ModuleName { get; }
}