namespace JMCore.Server.Storages.Abstract;

public abstract class DbVersionScriptsBase
{
    public abstract Version Version { get; }
    public abstract List<string> AllScripts { get; }
}