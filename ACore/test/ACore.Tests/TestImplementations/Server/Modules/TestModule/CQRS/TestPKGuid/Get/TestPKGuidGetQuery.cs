﻿using ACore.Base.CQRS.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Get;

public class TestPKGuidGetQuery: TestModuleRequest<Result<TestPKGuidData[]>>
{
  
}