using JMCore.Server.Storages.Base.EF;

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Scripts.Postgres;

public class ScriptRegistrations : DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions
    {
        get
        {
            var all = new List<DbVersionScriptsBase>
            {
                new V1_0_1_1TestTable(),
                new V1_0_1_2TestAuditTables(),
                new V1_0_1_3TestAuditTypes(),
                new V1_0_1_4TestPK()
            };
            return all;
        }
    }
}