﻿using ACore.Base.CQRS.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;

public class TestPKGuidSaveCommand(TestPKGuidData data): TestModuleRequest<Result>
{
  public TestPKGuidData Data => data;
}