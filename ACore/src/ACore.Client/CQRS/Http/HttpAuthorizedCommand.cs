using ACore.Client.Services.Http;
using ACore.Models.BaseRR;
using MediatR;

namespace ACore.Client.CQRS.Http;

public class HttpAuthorizedCommand<TResponse> : HttpSendCommandBase, IRequest<TResponse>
    where TResponse : ApiResponseBase, new()
{
    public HttpAuthorizedCommand(ApiMethod method, string url, ApiRequestBase apiRequest, Type type) : base(HttpClientTypeEnum.Authorized, method, url, apiRequest, type)
    {
    }
}