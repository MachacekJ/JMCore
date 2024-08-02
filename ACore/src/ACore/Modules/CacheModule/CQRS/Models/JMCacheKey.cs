namespace ACore.Modules.CacheModule.CQRS.Models;

public class JMCacheKey(int category, string key)
{
    public int Category { get; } = category;
    public string Key { get; } = key;

   

    public static JMCacheKey Create(int category, string key)
    {
        return new JMCacheKey(category, key);
    }

    public override string ToString()
    {
        return $"C:{Category}K:{Key}";
    }
}