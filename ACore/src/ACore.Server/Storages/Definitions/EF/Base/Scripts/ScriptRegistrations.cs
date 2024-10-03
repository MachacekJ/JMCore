namespace ACore.Server.Storages.Definitions.EF.Base.Scripts;

public class ScriptRegistrations : DbScriptBase
{
  public override IEnumerable<DbVersionScriptsBase> AllVersions => new List<DbVersionScriptsBase>();
}