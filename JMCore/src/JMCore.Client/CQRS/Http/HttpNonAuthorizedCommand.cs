using JMCore.Client.Services.Http;
using JMCore.Models.BaseRR;
using MediatR;

namespace JMCore.Client.CQRS.Http;

public class HttpNonAuthorizedCommand<TResponse>(ApiMethod method, string url, ApiRequestBase apiRequest, Type type)
    : HttpSendCommandBase(HttpClientTypeEnum.NonAuthorized, method, url, apiRequest, type), IRequest<TResponse>
    where TResponse : ApiResponseBase, new();

public class HttpNonAuthorizedCommand(ApiMethod method, string url, ApiRequestBase apiRequest, Type type)
    : HttpSendCommandBase(HttpClientTypeEnum.NonAuthorized, method, url, apiRequest, type), IRequest<ApiResponseBase>;