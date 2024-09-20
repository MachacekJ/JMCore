using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using MediatR;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS;

public class TestModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result
{
  
}

public class TestModuleHashEntityQueryRequest<TResponse>(bool withEntityHash) : HashEntityQueryRequest<TResponse>(withEntityHash)
  where TResponse : Result
{
  
}

public class TestModuleHashEntityCommandRequest<TResponse>(string? hashToCheck) : HashEntityCommandRequest<TResponse>(hashToCheck)
  where TResponse : Result
{
  
}