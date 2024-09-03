using ACore.Server.Storages.Scripts;

namespace ACore.Server.Modules.SettingModule.Storage.SQL.PG.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_1SettingsTable : DbVersionScriptsBase
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
    key VARCHAR(256) NOT NULL,
    value TEXT NOT NULL,
    is_system BOOL
);
"

      ];
      return l;
    }
  }
}