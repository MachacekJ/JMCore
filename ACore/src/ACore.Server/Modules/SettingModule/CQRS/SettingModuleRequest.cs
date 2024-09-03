using ACore.CQRS;

namespace ACore.Server.Modules.SettingModule.CQRS;

public class SettingModuleRequest<TResponse> : IResultRequest<TResponse>;
public class SettingModuleRequest : IResultRequest;