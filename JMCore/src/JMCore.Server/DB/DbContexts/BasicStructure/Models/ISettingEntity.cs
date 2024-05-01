namespace JMCore.Server.DB.DbContexts.BasicStructure.Models;

public interface ISettingEntity
{
  public int Id { get; set; }
  public string Key { get; set; }
  public string Value { get; set; }
  public bool? IsSystem { get; set; }
}