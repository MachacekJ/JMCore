using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Storages.EF;

public abstract class DbVersionScriptsBase
{
    public abstract Version Version { get; }
    public virtual List<string> AllScripts { get; } = new();

    public virtual void AfterScriptRunCode<T>(T impl, DbContextOptions options, ILogger<DbContextBase> logger) where T : IStorage
    {
      
    }
}