namespace JMCore.Server.MongoStorage.BasicModule.Models;

public class SettingCollection : BaseCollection
{
  public string Key { get; set; } = null!;
  public string Value { get; set; } = null!;
  
  public bool? IsSystem { get; set; }
}