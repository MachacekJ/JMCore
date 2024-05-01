using JMCore.Server.Storages.Abstract;

namespace JMCore.Server.Storages.DbContexts.AuditStructure.Scripts.Postgres
{
    public class ScriptRegistrations : DbScriptBase
    {
        public override IEnumerable<DbVersionScriptsBase> AllVersions
        {
            get
            {
                var all = new List<DbVersionScriptsBase>();
                all.Add(new V1_0_0_1AuditTables());
                return all;
            }
        }
    }
}
