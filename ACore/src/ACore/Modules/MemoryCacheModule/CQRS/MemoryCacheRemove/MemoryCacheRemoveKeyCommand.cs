﻿using ACore.Base.Cache;
using ACore.Base.CQRS.Results;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheModuleRemoveKeyCommand(CacheKey key) : MemoryCacheModuleRequest<Result<bool>>
{
  public CacheKey? Key { get; } = key;
}