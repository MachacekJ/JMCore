namespace ACore.Base.CQRS.Configuration;

public class ModuleOptionsBuilder
{
  private bool _isActive;

  protected bool IsActive => _isActive;
  
  public void Activate()
  {
    _isActive = true;
  }
}