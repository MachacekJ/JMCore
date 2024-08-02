namespace ACore.Server.Storages.EF;

public class ScriptRegistrations : DbScriptBase
{
  public override IEnumerable<DbVersionScriptsBase> AllVersions => new List<DbVersionScriptsBase>();
}