using JMCore.Server.Storages.Base.EF;

namespace JMCore.Server.PGStorage.LocalizationModule.Scripts
{
    // ReSharper disable once InconsistentNaming
    public class V1_0_0_01LocalizationTable : DbVersionScriptsBase
    {
        public override Version Version => new("1.0.0.1");

        public override List<string> AllScripts
        {
            get
            {

                List<string> l = new()
                {
	                @"
CREATE TABLE localization
(
    localization_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    msgid VARCHAR(255) NOT NULL,
    translation TEXT NOT NULL,
    lcid INT NOT NULL,
    contextid VARCHAR(1024) NOT NULL,
    scope INT NOT NULL,
    changed TIMESTAMP
);

CREATE INDEX idx_localization_c_scope 
    ON localization (scope);

CREATE INDEX idx_localization_c_msgid
    ON localization (msgid);
"
                };
                return l;
            }
        }
    }
}