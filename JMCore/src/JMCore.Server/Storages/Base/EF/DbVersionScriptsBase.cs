namespace JMCore.Server.Storages.Base.EF;

public abstract class DbVersionScriptsBase
{
    public abstract Version Version { get; }
    public abstract List<string> AllScripts { get; }
}