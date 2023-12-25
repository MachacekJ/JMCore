namespace JMCore.Server.DB.Abstract;

public abstract class DbVersionScriptsBase
{
    public abstract Version Version { get; }
    public abstract List<string> AllScripts { get; }
}