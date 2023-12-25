namespace JMCore.Services.JMCache;

public class JMCacheValue
{
    public object? Value { get; }

    public JMCacheValue(object? value)
    {
        Value = value;
    }
}