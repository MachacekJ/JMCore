using ACore.Server.Storages.EF;

namespace ACore.Server.PGStorage.LocalizationModule.Scripts
{
    public class ScriptRegistrations : DbScriptBase
    {
        public override IEnumerable<DbVersionScriptsBase> AllVersions
        {
            get
            {
                var all = new List<DbVersionScriptsBase>();
                all.Add(new V1_0_0_01LocalizationTable());
                return all;
            }
        }
    }
}

