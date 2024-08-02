using System.Threading;
using System.Threading.Tasks;
using ACore.Blazor.CQRS.LocalStorage.Models;
using FluentAssertions;
using ACore.Blazor.Components.SideBar.LeftSideBar;
using MediatR;
using Moq;
using LeftMenuContent = ACore.Blazor.Components.SideBar.LeftSideBar.LeftMenuContent;

namespace ACore.Blazor.BUnit.SideBarT.LeftSideBarT;

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