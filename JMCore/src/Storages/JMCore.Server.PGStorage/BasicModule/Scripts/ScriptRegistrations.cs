﻿using JMCore.Server.Storages.Base.EF;

namespace JMCore.Server.PGStorage.BasicModule.Scripts
{
    public class ScriptRegistrations : DbScriptBase
    {
        public override IEnumerable<DbVersionScriptsBase> AllVersions
        {
            get
            {
                var all = new List<DbVersionScriptsBase>();
                all.Add(new V1_0_0_1SettingsTable());
                return all;
            }
        }
    }
}