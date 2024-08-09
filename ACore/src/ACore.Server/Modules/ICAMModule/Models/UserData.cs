namespace ACore.Server.Modules.ICAMModule.Models;

public class UserData
{
  private readonly UserTypeEnum _userType;
  private readonly string _id;
  private readonly string _name;

  public UserData(UserTypeEnum userType, string id, string name)
  {
    CheckForbiddenLettres(id);
    _userType = userType;
    _id = id;
    _name = name;
  }

  public UserTypeEnum UserType => _userType;
  public string Id => _id;
  public string Name => _name;

  
  public override string ToString()
  {
    return $"{_userType.ToCode()}^{_id}";
  }
  
  private void CheckForbiddenLettres(string key)
  {
    if (key.Contains('^'))
      throw new Exception($"Cache key '{key}' contains forbidden letter '^'.");
  }
}