using JMCore.Server.Storages.Abstract;

namespace JMCore.Server.Storages.DbContexts.LocalizeStructure.Scripts.Postgres
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

