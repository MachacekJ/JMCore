﻿using JMCore.Server.Storages.EF;

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages.PG.Scripts;

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
                new V1_0_1_4TestPK(),
                new V1_0_1_5TestParentChild()
            };
            return all;
        }
    }
}