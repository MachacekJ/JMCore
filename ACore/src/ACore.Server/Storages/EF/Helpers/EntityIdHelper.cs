namespace ACore.Server.Storages.EF.Helpers;

public class EntityIdHelper
{
  public static bool IsNew<TU>(TU id)
  {
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);

    return typeof(TU) switch
    {
      { } entityType when entityType == typeof(int) => (int)Convert.ChangeType(id, typeof(int)) == 0,
      { } entityType when entityType == typeof(long) => (long)Convert.ChangeType(id, typeof(long)) == 0,
      { } entityType when entityType == typeof(string) => string.IsNullOrEmpty((string)Convert.ChangeType(id, typeof(string))),
      { } entityType when entityType == typeof(Guid) => (Guid)Convert.ChangeType(id, typeof(Guid)) == Guid.Empty,
      _ => throw new Exception("Unknown primary data type {}")
    };
  }
  

  
}