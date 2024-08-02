using ACore.Client.Services.Http;
using ACore.Models.BaseRR;
using MediatR;

namespace ACore.Client.CQRS.Http;

public class HttpNonAuthorizedCommand<TResponse>(ApiMethod method, string url, ApiRequestBase apiRequest, Type type)
    : HttpSendCommandBase(HttpClientTypeEnum.NonAuthorized, method, url, apiRequest, type), IRequest<TResponse>
    where TResponse : ApiResponseBase, new();

public class HttpNonAuthorizedCommand(ApiMethod method, string url, ApiRequestBase apiRequest, Type type)
    : HttpSendCommandBase(HttpClientTypeEnum.NonAuthorized, method, url, apiRequest, type), IRequest<ApiResponseBase>;