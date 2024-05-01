namespace JMCore.Server.Storages.Abstract;

public interface IDbContextBase
{
  Task UpdateDbAsync();
  DbScriptBase SqlScripts { get; }
  string DbContextName { get; }
}