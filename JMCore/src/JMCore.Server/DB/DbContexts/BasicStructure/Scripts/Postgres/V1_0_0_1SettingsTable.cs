using JMCore.Server.DB.Abstract;

namespace JMCore.Server.DB.DbContexts.BasicStructure.Scripts.Postgres
{
    // ReSharper disable once InconsistentNaming
    public class V1_0_0_1SettingsTable : DbVersionScriptsBase
    {
        public override Version Version
        {
            get
            {
                return new Version("1.0.0.1");
            }
        }

        public override List<string> AllScripts
        {
            get
            {

                List<string> l = new ();
                l.Add(@"
CREATE TABLE setting
(
    setting_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    key VARCHAR(100) NOT NULL,
    value VARCHAR NOT NULL,
    is_system BOOL
);
");


                return l;
            }
        }
    }
}
