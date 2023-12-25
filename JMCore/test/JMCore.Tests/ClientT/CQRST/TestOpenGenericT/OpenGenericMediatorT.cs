using System.Reflection;
using Autofac;
using FluentAssertions;
using JMCore.Tests.ClientT.ServicesT.HttpT;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JMCore.Tests.ClientT.CQRST.TestOpenGenericT;

public class OpenGenericMediatorT : HttpBaseT
{
    // ReSharper disable once RedundantOverriddenMember
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
       // sc.AddMediatrExplicitHandlers();
    }

    protected override void RegisterAutofacContainer(ServiceCollection services, ContainerBuilder containerBuilder)
    {
        base.RegisterAutofacContainer(services, containerBuilder);
        containerBuilder.RegisterGeneric(typeof(CountFruitRequestHandler<,>)).AsImplementedInterfaces();
    }

    [Fact]
    public async Task ResolvedHandlerForApple_ReturnsExpected()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.

        await RunTestAsync(method, async () =>
        {
            // Arrange

            var request = new CountFruitCommand<Apple, AppleCode>() { Count = 1 };

            // Act
            var actual = await Mediator.Send(request);

            // Assert
            actual.Weight.Should().Be(10);


            var request2 = new CountFruitCommand<Orange, OrangeCode>() { Count = 2 };

            // Act
            var actual2 = await Mediator.Send(request2);

            // Assert
            actual2.Weight.Should().Be(12);
        });
    }
}

public static class ExtensionsServiceCollection
{
    public static IServiceCollection AddMediatrExplicitHandlers(this IServiceCollection services)
    {
        services
            .AddTransient<IRequestHandler<CountFruitCommand<Apple, AppleCode>, Apple>, CountFruitRequestHandler<Apple, AppleCode>>()
            .AddTransient<IRequestHandler<CountFruitCommand<Orange, OrangeCode>, Orange>, CountFruitRequestHandler<Orange, OrangeCode>>();

        return services;
    }
}