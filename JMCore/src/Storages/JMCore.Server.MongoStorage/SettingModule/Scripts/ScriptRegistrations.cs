﻿using JMCore.Server.Storages.EF;

namespace JMCore.Server.MongoStorage.SettingModule.Scripts
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
