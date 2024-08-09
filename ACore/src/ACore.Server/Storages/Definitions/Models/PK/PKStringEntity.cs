namespace ACore.Server.Storages.Definitions.Models.PK;

public abstract class PKStringEntity() : PKEntity<string>(EmptyId)
{
  public static string NewId => Guid.NewGuid().ToString();
  public static string EmptyId => string.Empty;
}