namespace JMCore.Server.Storages.Base.EF;

public class ScriptRegistrations : DbScriptBase
{
  public override IEnumerable<DbVersionScriptsBase> AllVersions => new List<DbVersionScriptsBase>();
}