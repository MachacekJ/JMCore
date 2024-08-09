using ACore.Server.Storages.Definitions.EF.Base.Scripts;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG.Scripts;

internal class ScriptRegistrations : DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<DbVersionScriptsBase> { new V1_0_0_1SettingsTable() };
            return all;
        }
    }
}