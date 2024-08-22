using ACore.Server.Storages.EF;

namespace ACore.Server.Modules.SettingModule.Storage.PG.Scripts;

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