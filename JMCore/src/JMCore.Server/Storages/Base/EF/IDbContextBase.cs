namespace JMCore.Server.Storages.Base.EF;

public interface IDbContextBase
{
  Task Init();
  DbScriptBase UpdateScripts { get; }
}