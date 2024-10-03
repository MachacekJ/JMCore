namespace ACore.Server.Storages.Definitions.Models.PK;

public abstract class PKLongEntity(): PKEntity<long>(EmptyId)
{
  public static long NewId => 0;
  public static long EmptyId => 0;
}