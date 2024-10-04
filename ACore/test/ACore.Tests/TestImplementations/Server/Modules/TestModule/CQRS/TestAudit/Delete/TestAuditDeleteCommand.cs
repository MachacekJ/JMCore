﻿using ACore.Base.CQRS.Results;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Delete;

public class TestAuditDeleteCommand<T>(T id): TestModuleRequest<Result>
{
  public T Id => id;
}