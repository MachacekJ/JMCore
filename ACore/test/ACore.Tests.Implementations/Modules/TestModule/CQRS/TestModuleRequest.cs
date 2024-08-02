using ACore.CQRS;

namespace ACore.Tests.Implementations.Modules.TestModule.CQRS;

public class TestModuleRequest<TResponse> : LoggedRequest<TResponse>;