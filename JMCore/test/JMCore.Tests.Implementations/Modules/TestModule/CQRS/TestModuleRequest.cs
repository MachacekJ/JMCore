using JMCore.CQRS;

namespace JMCore.Tests.Implementations.Modules.TestModule.CQRS;

public class TestModuleRequest<TResponse> : LoggedRequest<TResponse>;