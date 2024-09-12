using ACore.Services.Cache;
using ACore.Services.Cache.Models;

namespace ACore.Modules.MemoryCacheModule.Storages;

public interface IMemoryCacheStorage
{
  CacheCategory[] Categories { get; }
  TItem? Get<TItem>(CacheKey key);
  void Set<TItem>(CacheKey key, TItem value);
  bool TryGetValue<TItem>(CacheKey key, out TItem? value);
  void Remove(CacheKey key);
  void RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory);
}