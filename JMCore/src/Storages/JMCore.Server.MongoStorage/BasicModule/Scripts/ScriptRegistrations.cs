using JMCore.Server.Storages.Base.EF;

namespace JMCore.Server.MongoStorage.BasicModule.Scripts
{
    public class ScriptRegistrations : DbScriptBase
    {
        public override IEnumerable<DbVersionScriptsBase> AllVersions
        {
            get
            {
                var all = new List<DbVersionScriptsBase>
                {
                    new V1_0_0_1SettingsCollection()
                };
                return all;
            }
        }
    }
}
