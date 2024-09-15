using System.Reflection;
using ACore.Client.CQRS.Http;
using ACore.Client.Services.Http;
using ACore.ResX;
using ACore.Tests.BaseInfrastructure;
using ACore.Tests.BaseInfrastructure.Models;
using ACore.Tests.Client.ServicesT.HttpT.Implementations;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;

namespace ACore.Tests.Client.ServicesT.HttpT;

public class HttpBaseTests : BaseTests
{
    protected const string FakeUrl = "http://fake/";
    private List<FakeHttpClientConfiguration> _mockedResponses = new();

    protected override void RegisterServices( ServiceCollection sc)
    {
        base.RegisterServices(sc);
        var aa = new JMHttpFakeClientFactory(_mockedResponses);
        sc.AddSingleton<IJMHttpClientFactory>(aa);

        sc.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(IJMHttpClientFactory));
        });
        
        var mockedLocalizations = new Mock<IStringLocalizer<ResX_Errors>>();
        
        var badRequestKey = ResX_Errors.ApiResponseBaseStatusCode_ERROR_BADREQUEST_RESPONSE;
        var badRequestString = new LocalizedString(badRequestKey, badRequestKey);
        var otherRequestKey = ResX_Errors.ApiResponseBaseStatusCode_ERROR_OTHER_RESPONSE;
        var otherRequestString = new LocalizedString(otherRequestKey, otherRequestKey);
        var internalErrorKey = ResX_Errors.ApiResponseBaseStatusCode_ERROR_INTERNALSERVERERROR_RESPONSE;
        var internalErrorString = new LocalizedString(internalErrorKey, internalErrorKey);
        var unauthorizedKey = ResX_Errors.ApiResponseBaseStatusCode_ERROR_UNAUTHORIZED_RESPONSE;
        var unauthorizedString = new LocalizedString(unauthorizedKey, unauthorizedKey);
        var parseKey = ResX_Errors.ApiResponseBaseStatusCode_ERROR_PARSEJSON;
        var parseString = new LocalizedString(parseKey, parseKey);
        mockedLocalizations.Setup(l => l[badRequestKey]).Returns(badRequestString);
        mockedLocalizations.Setup(l => l[otherRequestKey]).Returns(otherRequestString);
        mockedLocalizations.Setup(l => l[internalErrorKey]).Returns(internalErrorString);
        mockedLocalizations.Setup(l => l[unauthorizedKey]).Returns(unauthorizedString);
        mockedLocalizations.Setup(l => l[parseKey]).Returns(parseString);
        sc.AddSingleton(mockedLocalizations.Object);
        
    }

    protected override void SetContainer(ContainerBuilder containerBuilder)
    {
        base.SetContainer(containerBuilder);
        containerBuilder.RegisterGeneric(typeof(HttpAuthorizedHandler<>)).AsImplementedInterfaces();
        containerBuilder.RegisterGeneric(typeof(HttpNonAuthorizedHandler<>)).AsImplementedInterfaces();
    }


    protected async Task RunTestAsync(MemberInfo? method, List<FakeHttpClientConfiguration> mockedResponses, Func<Task> testCode)
    {
        if (method == null)
            throw new ArgumentException($"Method name is null {nameof(RunTestAsync)}");

        _mockedResponses = mockedResponses;

        await RunTestAsync(new TestData(method), testCode);
    }
}