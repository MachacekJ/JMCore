using System;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Telerik.Blazor;
using Telerik.SvgIcons;
using Image = JMCore.Blazor.Components.General.Image;

namespace JMCore.Blazor.BUnit.ComponentsT.GeneralT;

public class ImageTests : BUnitTestBase
{
    [Fact]
    public void BaseTest()
    {
        // Arrange
        RegisterMediatR();
        RegisterAppState();

        var clickEvent = 0;
        Action<MouseEventArgs> onClickHandler = _ => { clickEvent++; };

        // Act
        var cut = RenderComponent<Image>(pa => pa
            .Add(p => p.Icon, SvgIcon.Anchor)
            .Add(p => p.Size, ThemeConstants.SvgIcon.Size.Medium)
            .Add(p => p.OnClick, onClickHandler));

        var button = cut.Find("button");
        button.Click();

        // Assert
        button.ClassName.Should().Contain("k-button");
        clickEvent.Should().Be(1);
    }
}