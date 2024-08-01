using JMCore.Server.Storages.EF;

namespace JMCore.Server.PGStorage.AuditModule.Scripts
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
