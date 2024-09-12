using ACore.Models;
using ACore.Services.Cache.Models;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheModuleRemoveKeyCommand(CacheKey key) : MemoryCacheModuleRequest<Result<bool>>
{
  public CacheKey? Key { get; } = key;
}