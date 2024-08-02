﻿using ACore.AppTest.Modules.TestModule.CQRS.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid;

internal class TestPKGuidGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKGuidGetQuery, IEnumerable<TestPKGuidData>>(storageResolver)
{
  public override async Task<IEnumerable<TestPKGuidData>> Handle(TestPKGuidGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().AllTestPKGuid()).Select(TestPKGuidData.Create);
  }
}