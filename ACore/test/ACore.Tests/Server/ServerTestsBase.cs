using ACore.Server.Configuration;
using ACore.Tests.Base;
using Autofac;

namespace ACore.Tests.Server;

public abstract class ServerTestsBase : TestsBase
{
  protected override void SetContainer(ContainerBuilder containerBuilder)
  {
    base.SetContainer(containerBuilder);
    containerBuilder.ConfigureAutofacACoreServer();
  }
}