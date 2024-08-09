using ACore.Base.CQRS.Base;
using ACore.Base.CQRS.Results;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS;

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