using JMCore.Client.Services.Http;
using JMCore.Models.BaseRR;
using JMCore.ResX;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace JMCore.Client.CQRS.Http;

public class HttpNonAuthorizedHandler<TResponse>(IJMHttpClientFactory httpClientFactory, IStringLocalizer<ResX_Errors> resxCoreErrors, ILogger<HttpNonAuthorizedHandler<TResponse>> log)
    : HttpSendHandlerBase<TResponse>(httpClientFactory, resxCoreErrors, log), IRequestHandler<HttpNonAuthorizedCommand<TResponse>, TResponse>
    where TResponse : ApiResponseBase, new()
{
    public async Task<TResponse> Handle(HttpNonAuthorizedCommand<TResponse> request, CancellationToken cancellationToken)
    {
        return await base.Handle(request, cancellationToken);
    }
}

public class HttpNonAuthorizedHandler(IJMHttpClientFactory httpClientFactory, IStringLocalizer<ResX_Errors> resxCoreErrors, ILogger<HttpNonAuthorizedHandler> log)
    : HttpSendHandlerBase<ApiResponseBase>(httpClientFactory, resxCoreErrors, log), IRequestHandler<HttpNonAuthorizedCommand, ApiResponseBase>
{
    public async Task<ApiResponseBase> Handle(HttpNonAuthorizedCommand request, CancellationToken cancellationToken)
    {
        return await base.Handle(request, cancellationToken);
    }
}