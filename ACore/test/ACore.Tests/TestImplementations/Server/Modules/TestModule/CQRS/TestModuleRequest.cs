using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages.Attributes;
using MediatR;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS;

public class TestModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result
{
  
}

public class TestModuleQueryRequest<TResponse> : EntityQueryRequest<TResponse>
  where TResponse : Result;

public class TestModuleCommandRequest<TResponse>(string? hashToCheck) : EntityCommandRequest<TResponse>(hashToCheck)
  where TResponse : Result
{
  
}