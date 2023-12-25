using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace JMCore.Services.JMCache;

public interface IJMCache
{
    IJMCacheCategories Categories { get; }
    TItem? Get<TItem>(JMCacheKey key);
    void Set<TItem>(JMCacheKey key, TItem value);
    void Set<TItem>(JMCacheKey key, TItem value, MemoryCacheEntryOptions options);
    void Set<TItem>(JMCacheKey key, TItem value, IChangeToken expirationToken);
    void Set<TItem>(JMCacheKey key, TItem value, DateTimeOffset absoluteExpiration);
    void Set<TItem>(JMCacheKey key, TItem value, TimeSpan absoluteExpirationRelativeToNow);
    bool TryGetValue<TItem>(JMCacheKey key, out TItem? value);
    void Remove(JMCacheKey key);
    void RemoveCategory(int categoryId, string? prefix = null);
}