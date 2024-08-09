namespace ACore.Server.Storages.Definitions.EF.Base.Scripts;

public abstract class DbScriptBase
{
    public abstract IEnumerable<DbVersionScriptsBase> AllVersions { get; }
}

