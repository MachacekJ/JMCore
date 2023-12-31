﻿using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.BasicStructure;

public interface IBasicDbContext : IDbContextBase
{
    Task<string?> Setting_GetAsync(string key, bool isRequired = true);
    Task Setting_SaveAsync(string key, string value, bool isSystem = false);
}