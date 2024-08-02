using ACore.Server.Storages.Base.EF;

namespace ACore.Server.MSSQLStorage.BasicStructure.Scripts
{
    public class ScriptRegistrations : DbScriptBase
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
}
