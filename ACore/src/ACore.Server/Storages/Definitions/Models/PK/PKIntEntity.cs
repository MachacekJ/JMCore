namespace ACore.Server.Storages.Definitions.Models.PK;

public abstract class PKIntEntity() : PKEntity<int>(EmptyId)
{
  public static int NewId => 0;
  public static int EmptyId => 0;
}