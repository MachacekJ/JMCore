using ACore.Server.Storages.Scripts;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG.Scripts;

internal class ScriptRegistrations : DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<DbVersionScriptsBase>();
            all.Add(new V1_0_0_1SettingsTable());
            return all;
        }
    }
}