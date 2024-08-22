using ACore.Server.Storages.EF;

namespace ACore.Server.Modules.SettingModule.Storage.Mongo.Scripts;

internal class ScriptRegistrations : DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<DbVersionScriptsBase>
            {
                new V1_0_0_1SettingsCollection()
            };
            return all;
        }
    }
}