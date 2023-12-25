using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.LocalizeStructure.Scripts
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

