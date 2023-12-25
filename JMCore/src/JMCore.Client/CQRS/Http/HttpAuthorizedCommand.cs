using JMCore.Client.Services.Http;
using JMCore.Models.BaseRR;
using MediatR;

namespace JMCore.Client.CQRS.Http;

public class HttpAuthorizedCommand<TResponse> : HttpSendCommandBase, IRequest<TResponse>
    where TResponse : ApiResponseBase, new()
{
    public HttpAuthorizedCommand(ApiMethod method, string url, ApiRequestBase apiRequest, Type type) : base(HttpClientTypeEnum.Authorized, method, url, apiRequest, type)
    {
    }
}