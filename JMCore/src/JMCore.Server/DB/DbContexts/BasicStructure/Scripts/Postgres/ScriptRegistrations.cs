using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.BasicStructure.Scripts.Postgres
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
