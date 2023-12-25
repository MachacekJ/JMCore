namespace JMCore.Services.JMCache;

public class JMCacheKey
{
    public int Category { get; }
    public string Key { get; }

    private JMCacheKey(int category, string key)
    {
        Category = category;
        Key = key;
    }

    public static JMCacheKey Create(int category, string key)
    {
        return new JMCacheKey(category, key);
    }

    public override string ToString()
    {
        return $"C:{Category}K:{Key}";
    }
}