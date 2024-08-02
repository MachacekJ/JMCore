using ACore.Client.Services.Http;
using ACore.Models.BaseRR;
using ACore.ResX;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ACore.Client.CQRS.Http;

public class HttpAuthorizedHandler<TResponse> : HttpSendHandlerBase<TResponse>
    , IRequestHandler<HttpAuthorizedCommand<TResponse>, TResponse>
    where TResponse : ApiResponseBase, new()
{

    public HttpAuthorizedHandler(IJMHttpClientFactory httpClientFactory, IStringLocalizer<ResX_Errors> resxCoreErrors, ILogger<HttpAuthorizedHandler<TResponse>> log) : base(httpClientFactory, resxCoreErrors, log)
    {
    }

    public async Task<TResponse> Handle(HttpAuthorizedCommand<TResponse> request, CancellationToken cancellationToken)
    {
        return await base.Handle(request, cancellationToken);
    }
}