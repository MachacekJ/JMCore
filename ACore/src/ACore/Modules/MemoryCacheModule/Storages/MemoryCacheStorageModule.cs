using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using ACore.Modules.MemoryCacheModule.Configuration;
using ACore.Services.Cache;
using ACore.Services.Cache.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ACore.Modules.MemoryCacheModule.Storages;

public class MemoryCacheStorageModule(IMemoryCache memoryCache, MemoryCacheModuleOptions memoryCacheModuleOptions) : IMemoryCacheStorage
{
  public CacheCategory[] Categories => memoryCacheModuleOptions.Categories.ToArray();

  public TItem? Get<TItem>(CacheKey key)
  {
    return memoryCache.Get<TItem>(GetKey(key));
  }

  public void Set<TItem>(CacheKey key, TItem value)
  {
    memoryCache.Set(GetKey(key), value);
  }
  
  public bool TryGetValue<TItem>(CacheKey key, out TItem? value)
  {
    var res = memoryCache.TryGetValue(GetKey(key), out TItem? vall);
    value = vall;
    return res;
  }

  public void Remove(CacheKey key)
  {
    memoryCache.Remove(GetKey(key));
  }

  public void RemoveCategory(CacheCategory mainCategory, CacheCategory? subCategory)
  {
    var categoryKey = mainCategory.CategoryNameKey;
    var keyPrefix = subCategory?.CategoryNameKey;
    var keys = memoryCache.GetKeys<string>();
    foreach (var key in keys.Where(i => keyPrefix == null
               ? i.StartsWith($"C:{categoryKey}^")
               : i.StartsWith($"C:{categoryKey}^S:{keyPrefix}^")
             ).ToList())
    {
      memoryCache.Remove(key);
    }
  }

  private string GetKey(CacheKey key)
  {
    if (memoryCacheModuleOptions.Categories.All(k => k.CategoryNameKey != key.MainCategory.CategoryNameKey))
      throw new Exception($"Cache - Category '{key.MainCategory}' is not created.");

    return key.ToString();
  }
}

//https://stackoverflow.com/questions/45597057/how-to-retrieve-a-list-of-memory-cache-keys-in-asp-net-core
public static class MemoryCacheExtensions
{
  #region Microsoft.Extensions.Caching.Memory_6_OR_OLDER

  private static readonly Lazy<Func<MemoryCache, object>> GetEntries6 =
    new Lazy<Func<MemoryCache, object>>(() => ((Func<MemoryCache, object>)Delegate.CreateDelegate(
      typeof(Func<MemoryCache, object>),
      typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)!.GetGetMethod(true)!,
      throwOnBindFailure: true)!));

  #endregion

  #region Microsoft.Extensions.Caching.Memory_7_OR_NEWER

  private static readonly Lazy<Func<MemoryCache, object>> GetCoherentState =
    new Lazy<Func<MemoryCache, object>>(() =>
      CreateGetter<MemoryCache, object>(typeof(MemoryCache)
        .GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance)));

  private static readonly Lazy<Func<object, IDictionary>> GetEntries7 =
    new Lazy<Func<object, IDictionary>>(() =>
      CreateGetter<object, IDictionary>(typeof(MemoryCache)
        .GetNestedType("CoherentState", BindingFlags.NonPublic)!
        .GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance)));

  private static Func<TParam, TReturn> CreateGetter<TParam, TReturn>(FieldInfo? field)
  {
    var methodName = $"{field!.ReflectedType!.FullName}.get_{field.Name}";
    var method = new DynamicMethod(methodName, typeof(TReturn), new[] { typeof(TParam) }, typeof(TParam), true);
    var ilGen = method.GetILGenerator();
    ilGen.Emit(OpCodes.Ldarg_0);
    ilGen.Emit(OpCodes.Ldfld, field);
    ilGen.Emit(OpCodes.Ret);
    return (Func<TParam, TReturn>)method.CreateDelegate(typeof(Func<TParam, TReturn>));
  }

  #endregion

  private static readonly Func<MemoryCache, IDictionary> GetEntries =
    Assembly.GetAssembly(typeof(MemoryCache))!.GetName().Version!.Major < 7
      ? cache => (IDictionary)GetEntries6.Value(cache)
      : cache => GetEntries7.Value(GetCoherentState.Value(cache));

  public static ICollection GetKeys(this IMemoryCache memoryCache) =>
    GetEntries((MemoryCache)memoryCache).Keys;

  public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
    memoryCache.GetKeys().OfType<T>();
}