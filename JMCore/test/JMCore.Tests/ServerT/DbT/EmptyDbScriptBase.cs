using JMCore.Server.Storages.Abstract;

namespace JMCore.Tests.ServerT.DbT;

public class EmptyDbScriptBase :DbScriptBase
{
    public override IEnumerable<DbVersionScriptsBase> AllVersions { get; } = new List<DbVersionScriptsBase>();
}