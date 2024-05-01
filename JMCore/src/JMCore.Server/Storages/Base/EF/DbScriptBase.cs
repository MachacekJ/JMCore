namespace JMCore.Server.Storages.Base.EF;

public abstract class DbScriptBase
{
    public abstract IEnumerable<DbVersionScriptsBase> AllVersions { get; }
}

