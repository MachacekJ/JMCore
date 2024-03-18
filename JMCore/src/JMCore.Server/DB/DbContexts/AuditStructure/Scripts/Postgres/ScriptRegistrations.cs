using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.AuditStructure.Scripts.Postgres
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
