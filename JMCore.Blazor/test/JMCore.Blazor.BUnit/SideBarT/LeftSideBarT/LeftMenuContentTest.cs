using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JMCore.Blazor.Components.SideBar.LeftSideBar;
using JMCore.Blazor.CQRS.LocalStorage.Models;
using MediatR;
using Moq;

namespace JMCore.Blazor.BUnit.SideBarT.LeftSideBarT;

public class LeftMenuContentTest : BUnitTestBase
{
    [Fact]
    public void Empty()
    {
        // Arrange
        var mockedMediator = new Mock<IMediator>();
        mockedMediator.Setup(s =>
             s.Send(It.IsAny<IRequest<LocalStorageData>>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new LocalStorageData()));

        RegisterMediatR(mockedMediator.Object);
        RegisterAppState();
        RegisterAppSettings();

        // Act
        var cut = RenderComponent<LeftMenuContent>();
        
        // Assert
        cut.Find(".k-panelbar").Should().NotBeNull();
    }
}