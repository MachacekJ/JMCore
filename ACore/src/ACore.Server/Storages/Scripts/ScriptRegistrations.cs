namespace ACore.Server.Storages.Scripts;

public class ScriptRegistrations : DbScriptBase
{
  public override IEnumerable<DbVersionScriptsBase> AllVersions => new List<DbVersionScriptsBase>();
}