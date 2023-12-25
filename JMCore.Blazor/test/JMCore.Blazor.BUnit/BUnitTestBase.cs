using JMCore.Blazor.BUnit.Implementations.IAppStartConfugurationT;
using JMCore.Blazor.Services.App;
using MediatR;
using Moq;

namespace JMCore.Blazor.BUnit;

public class BUnitTestBase : TestContext
{
    protected void RegisterMediatR(IMediator? mediator = null)
    {
        mediator ??= new Mock<IMediator>().Object;
        Services.AddSingleton(mediator);
    }

    protected void RegisterAppState(IAppState? appState = null)
    {
        if (appState == null)
        {
            var mocked = new Mock<IAppState>();
            mocked.SetupGet(a => a.AppSetting).Returns(new EmptyAppStartConfiguration());
            Services.AddSingleton(mocked.Object);
        }
        else
            Services.AddSingleton(appState);
    }

    protected void RegisterAppSettings(IAppStartConfiguration? appStartConfiguration = null)
    {
        appStartConfiguration ??= new Mock<IAppStartConfiguration>().Object;
        Services.AddSingleton(appStartConfiguration);
    }
}