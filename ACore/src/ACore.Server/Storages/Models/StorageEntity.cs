namespace ACore.Server.Storages.Models;

public class IntStorageEntity() : StorageEntity<int>(0)
{
}

public class LongStorageEntity(): StorageEntity<long>(0)
{
}

public class GuidStorageEntity() : StorageEntity<Guid>(Guid.Empty)
{
}

public class StringStorageEntity() : StorageEntity<string>(string.Empty)
{
}

public class StorageEntity<T>(T id)
{
  public T Id { get; set; } = id;
}