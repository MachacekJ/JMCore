namespace ACore.Base.Cache;

public class CacheValue(object? objectValue)
{
    public object? ObjectValue { get; } = objectValue;
    public T? GetValue<T>()
    {
        return (T?)ObjectValue;
    }
}