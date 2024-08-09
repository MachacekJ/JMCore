using ACore.Server.Configuration.Modules;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

public class SettingsDbModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(SettingsDbModule), isActive);