﻿using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Models;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Save;

public class TestAttributeAuditSaveCommand<T>(TestAttributeAuditData<T> data): TestModuleRequest<T>
{
  public TestAttributeAuditData<T> Data => data;
}