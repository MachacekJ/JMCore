using JMCore.Server.Storages.Abstract;

namespace JMCore.Server.Storages.DbContexts.LocalizeStructure.Scripts.MSSQL
{
    public class ScriptRegistrations : DbScriptBase
    {
        public override IEnumerable<DbVersionScriptsBase> AllVersions
        {
            get
            {
                var all = new List<DbVersionScriptsBase>();
                all.Add(new V1_0_0_01LocalizationTable());
                all.Add(new V1_0_0_02Index_Scope());
                all.Add(new V1_0_0_03Index_MsgId());
                return all;
            }
        }
    }
}

