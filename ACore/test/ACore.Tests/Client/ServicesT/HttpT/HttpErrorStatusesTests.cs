using System.Net;
using System.Reflection;
using ACore.Client.CQRS.Http;
using ACore.Client.Services.Http;
using ACore.Models.BaseRR;
using ACore.ResX;
using ACore.Tests.Client.ServicesT.HttpT.Implementations;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Client.ServicesT.HttpT;

public class HttpErrorStatusesTests : HttpBaseTests
{
    [Fact]
    public async Task Empty()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.
        var responses = new List<FakeHttpClientConfiguration>
        {
            new("{}", FakeUrl)
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var resObj = await Mediator.Send(new HttpNonAuthorizedCommand<ApiResponseBase>(ApiMethod.Get, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));

            // Assert.
            resObj.StatusCode.Should().Be(HttpStatusCode.OK);
            resObj.ServerErrorId.Should().BeNull();
            resObj.IsError.Should().Be(false);
            resObj.IsWarning.Should().Be(false);
            resObj.Code.Should().Be(ResponseBase.Code_None);
            resObj.Time.Should().NotBeNull();
            resObj.Message.Should().Be(string.Empty);
            resObj.ShortMessage.Should().Be(string.Empty);
        });
    }

    [Fact]
    public async Task ApiInternalServerErrorResponse()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.
        var responses = new List<FakeHttpClientConfiguration>
        {
            new(string.Empty, FakeUrl, HttpStatusCode.InternalServerError)
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var resObj = await Mediator.Send(new HttpNonAuthorizedCommand(ApiMethod.Get, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
      
            // Assert.
            resObj.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            resObj.ServerErrorId.Should().BeNull();
            resObj.IsError.Should().Be(true);
            resObj.IsWarning.Should().Be(false);
            resObj.Code.Should().Be(ApiResponseBase.Code_ErrorInternalServer);
            resObj.Time.Should().NotBeNull();
            resObj.Message.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_INTERNALSERVERERROR_RESPONSE);
            resObj.ShortMessage.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_INTERNALSERVERERROR_RESPONSE);
        });
    }
    
    [Fact]
    public async Task ApiBadRequestResponse()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.
        var responses = new List<FakeHttpClientConfiguration>
        {
            new(string.Empty, FakeUrl, HttpStatusCode.BadRequest)
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var resObj = await Mediator.Send(new HttpNonAuthorizedCommand(ApiMethod.Get, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
    
            // Assert.
            resObj.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            resObj.ServerErrorId.Should().BeNull();
            resObj.IsError.Should().Be(true);
            resObj.IsWarning.Should().Be(false);
            resObj.Code.Should().Be(ApiResponseBase.Code_ErrorBadRequest);
            resObj.Time.Should().NotBeNull();
            resObj.Message.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_BADREQUEST_RESPONSE);
            resObj.ShortMessage.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_BADREQUEST_RESPONSE);
        });
    }
    
    [Fact]
    public async Task ApiOtherRequestResponse()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.
        var responses = new List<FakeHttpClientConfiguration>
        {
            new(string.Empty, FakeUrl, HttpStatusCode.Forbidden)
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var resObj = await Mediator.Send(new HttpNonAuthorizedCommand(ApiMethod.Get, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
    
            // Assert.
            resObj.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            resObj.ServerErrorId.Should().BeNull();
            resObj.IsError.Should().Be(true);
            resObj.IsWarning.Should().Be(false);
            resObj.Code.Should().Be(ApiResponseBase.Code_ErrorOtherResponse);
            resObj.Time.Should().NotBeNull();
            resObj.Message.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_OTHER_RESPONSE);
            resObj.ShortMessage.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_OTHER_RESPONSE);
        });
    }
    
    [Fact]
    public async Task ApiUnAuthorizedResponse()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.
        var responses = new List<FakeHttpClientConfiguration>
        {
            new(string.Empty, FakeUrl, HttpStatusCode.Unauthorized)
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var resObj = await Mediator.Send(new HttpNonAuthorizedCommand(ApiMethod.Get, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
     
            // Assert.
            resObj.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            resObj.ServerErrorId.Should().BeNull();
            resObj.IsError.Should().Be(true);
            resObj.IsWarning.Should().Be(false);
            resObj.Code.Should().Be(ApiResponseBase.Code_ErrorUnauthorizedResponse);
            resObj.Time.Should().NotBeNull();
            resObj.Message.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_UNAUTHORIZED_RESPONSE);
            resObj.ShortMessage.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_UNAUTHORIZED_RESPONSE);
        });
    }
    
    [Fact]
    public async Task ParseError()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.
        var responses = new List<FakeHttpClientConfiguration>
        {
            new("{ewew {[]}", FakeUrl)
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var resObj = await Mediator.Send(new HttpNonAuthorizedCommand(ApiMethod.Get, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
     
            // Assert.
            resObj.StatusCode.Should().Be(HttpStatusCode.OK);
            resObj.ServerErrorId.Should().BeNull();
            resObj.IsError.Should().Be(true);
            resObj.IsWarning.Should().Be(false);
            resObj.Code.Should().Be(ApiResponseBase.Code_ErrorParseJson);
            resObj.Time.Should().NotBeNull();
            resObj.Message.Should().NotBeNull();
            resObj.Exception.Should().NotBeNull();
            resObj.ShortMessage.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_PARSEJSON);
        });
    }
    
    [Fact]
    public async Task JsonEmpty()
    {
        var method = MethodBase.GetCurrentMethod();
        // Arrange.
        var responses = new List<FakeHttpClientConfiguration>
        {
            new("", FakeUrl)
        };
        await RunTestAsync(method, responses, async () =>
        {
            // Action.
            var resObj = await Mediator.Send(new HttpNonAuthorizedCommand(ApiMethod.Get, FakeUrl, new ApiRequestBase(), typeof(ApiRequestBase)));
     
            // Assert.
            resObj.StatusCode.Should().Be(HttpStatusCode.OK);
            resObj.ServerErrorId.Should().BeNull();
            resObj.IsError.Should().Be(true);
            resObj.IsWarning.Should().Be(false);
            resObj.Code.Should().Be(ApiResponseBase.Code_ErrorParseJson);
            resObj.Time.Should().NotBeNull();
            resObj.Message.Should().NotBeNull();
            resObj.Exception.Should().NotBeNull();
            resObj.ShortMessage.Should().Be(ResX_Errors.ApiResponseBaseStatusCode_ERROR_PARSEJSON);
        });
    }
}