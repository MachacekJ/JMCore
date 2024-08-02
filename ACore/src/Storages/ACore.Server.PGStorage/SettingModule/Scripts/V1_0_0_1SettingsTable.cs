using ACore.Server.Storages.EF;

namespace ACore.Server.PGStorage.SettingModule.Scripts
{
  // ReSharper disable once InconsistentNaming
  public class V1_0_0_1SettingsTable : DbVersionScriptsBase
  {
    public override Version Version => new("1.0.0.1");

    public override List<string> AllScripts
    {
      get
      {
        List<string> l =
        [
          @"
CREATE TABLE setting
(
    setting_id INT GENERATED ALWAYS AS IDENTITY
        PRIMARY KEY,
    key VARCHAR(100) NOT NULL,
    value VARCHAR NOT NULL,
    is_system BOOL
);
"

        ];


        return l;
      }
    }
  }
}