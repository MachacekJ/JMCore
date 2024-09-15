﻿using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;

public class TestPKStringSaveCommand(TestPKStringData data): TestModuleRequest<Result>
{
  public TestPKStringData Data => data;
}