﻿using ACore.Base.CQRS.Results;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Get;

public class TestPKLongAuditGetQuery: TestModuleRequest<Result<TestPKLongData[]>>;