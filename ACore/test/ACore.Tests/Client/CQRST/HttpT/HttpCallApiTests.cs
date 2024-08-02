using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using ACore.Client.CQRS.Http;
using ACore.Client.Services.Http;
using ACore.Models.BaseRR;
using ACore.Tests.Client.ServicesT.HttpT;
using ACore.Tests.Client.ServicesT.HttpT.Implementations;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Client.CQRST.HttpT;

public class HttpCallApiTests : HttpBaseTests
{
    [Theory]
    [InlineData(ApiMethod.Get)]
    [InlineData(ApiMethod.Post)]
    [InlineData(ApiMethod.Put)]
    [InlineData(ApiMethod.Delete)]
    public async Task ApiMethodTest(ApiMethod apiMethod)
    {
        var testString = "fake test string ěšč ČŽÝÉÍŽŘ";
        var method = MethodBase.GetCurrentMethod();
        // Arrange.

        var responses = new List<FakeHttpClientConfiguration>
        {
            new(Newtonsoft.Json.JsonConvert.SerializeObject(new TestApiResponse
            {
                Test = testString,
                Code = TestApiResponse.Code_Test
            }), FakeUrl, HttpStatusCode.OK, (req, _) =>
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "token");
                if (req.Method == HttpMethod.Get || req.Method == HttpMethod.Delete)
                    req.RequestUri!.Query.Should().Contain("lcid");
            })
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var responseObject = await Mediator.Send(new HttpAuthorizedCommand<TestApiResponse>(apiMethod, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
            // Assert.
            responseObject.Test.Should().Be(testString);
            responseObject.Code.Should().Be(TestApiResponse.Code_Test);
            responseObject.IsWarning.Should().Be(true);
            responseObject.IsError.Should().Be(false);
        });
        
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var responseObject = await Mediator.Send(new HttpNonAuthorizedCommand<TestApiResponse>(apiMethod, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
            // Assert.
            responseObject.Test.Should().Be(testString);
            responseObject.Code.Should().Be(TestApiResponse.Code_Test);
            responseObject.IsWarning.Should().Be(true);
            responseObject.IsError.Should().Be(false);
        });
    }
    
    
    private class TestApiResponse : ApiResponseBase
    {
        public static readonly int Code_Test = 1000;
        public string Test { get; set; } = null!;
        
        public TestApiResponse()
        {
            AllStatus.Add(Code_Test, nameof(Code_Test));
        }
    }
}