using ACore.CQRS;

namespace ACore.AppTest.Modules.TestModule.CQRS;

public class TestModuleRequest<TResponse> : IResultRequest<TResponse>;
public class TestModuleRequest : IResultRequest;