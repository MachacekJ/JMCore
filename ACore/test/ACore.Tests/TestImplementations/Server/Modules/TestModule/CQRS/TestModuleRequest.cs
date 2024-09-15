using ACore.Base.CQRS.Models;
using MediatR;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS;

public class TestModuleRequest<TResponse> : IRequest<TResponse>
  where TResponse : Result;