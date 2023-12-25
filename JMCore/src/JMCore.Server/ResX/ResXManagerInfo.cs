using System.Resources;
using JMCore.Localizer;

namespace JMCore.Server.ResX;

public class ResXManagerInfo
{
    public string ContextId { get; }
    public ResourceManager ResourceManager { get; }
    public LocalizationScopeEnum Scope { get; }

    public ResXManagerInfo(string contextId, ResourceManager resourceManager, LocalizationScopeEnum scope)
    {
        ContextId = contextId;
        ResourceManager = resourceManager;
        Scope = scope;
    }
}