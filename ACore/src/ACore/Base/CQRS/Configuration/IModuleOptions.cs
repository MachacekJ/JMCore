namespace ACore.Base.CQRS.Configuration;

public interface IModuleOptions
{
  public string ModuleName { get; }
  public bool IsActive { get; }
}