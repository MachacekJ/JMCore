// using System.Reflection;
// using ACore.Tests.Client.ServicesT.HttpT;
// using Autofac;
// using FluentAssertions;
// using MediatR;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;
//
// namespace ACore.Tests.Client.CQRST.TestOpenGenericT;
//
// public class OpenGenericMediatorTests : HttpBaseTests
// {
//     // ReSharper disable once RedundantOverriddenMember
//     protected override void RegisterServices(ServiceCollection sc)
//     {
//         base.RegisterServices(sc);
//     }
//
//     protected override void SetContainer(ContainerBuilder containerBuilder)
//     {
//         base.SetContainer(containerBuilder);
//         containerBuilder.RegisterGeneric(typeof(CountFruitRequestHandler<,>)).AsImplementedInterfaces();
//     }
//
//     [Fact]
//     public async Task ResolvedHandlerForApple_ReturnsExpected()
//     {
//         var method = MethodBase.GetCurrentMethod();
//         // Arrange.
//
//         await RunTestAsync(method, async () =>
//         {
//             // Arrange
//
//             var request = new CountFruitCommand<Apple, AppleCode>() { Count = 1 };
//
//             // Act
//             var actual = await Mediator.Send(request);
//
//             // Assert
//             actual.Weight.Should().Be(10);
//
//
//             var request2 = new CountFruitCommand<Orange, OrangeCode>() { Count = 2 };
//
//             // Act
//             var actual2 = await Mediator.Send(request2);
//
//             // Assert
//             actual2.Weight.Should().Be(12);
//         });
//     }
// }
//
// public static class ExtensionsServiceCollection
// {
//     public static IServiceCollection AddMediatrExplicitHandlers(this IServiceCollection services)
//     {
//         services
//             .AddTransient<IRequestHandler<CountFruitCommand<Apple, AppleCode>, Apple>, CountFruitRequestHandler<Apple, AppleCode>>()
//             .AddTransient<IRequestHandler<CountFruitCommand<Orange, OrangeCode>, Orange>, CountFruitRequestHandler<Orange, OrangeCode>>();
//
//         return services;
//     }
// }