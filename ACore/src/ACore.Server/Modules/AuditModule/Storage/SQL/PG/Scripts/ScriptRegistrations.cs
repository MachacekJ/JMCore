using ACore.Server.Storages.Definitions.EF.Base.Scripts;

namespace ACore.Server.Modules.AuditModule.Storage.SQL.PG.Scripts;

internal class ScriptRegistrations : DbScriptBase
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