using ACore.Server.Configuration.Modules;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Configuration;

public class TestModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(TestModule), isActive);