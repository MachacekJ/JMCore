using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using JMCore.Client.Services.Http;
using JMCore.Extensions;
using JMCore.Models.BaseRR;
using JMCore.ResX;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace JMCore.Client.CQRS.Http;

public class HttpSendHandlerBase<TResponse>
    where TResponse : ApiResponseBase, new()
{
    private readonly IJMHttpClientFactory _httpClientFactory;
    private readonly IStringLocalizer<ResX_Errors> _resxCoreErrors;
    private readonly ILogger<HttpSendHandlerBase<TResponse>> _logger;

    protected HttpSendHandlerBase(IJMHttpClientFactory httpClientFactory, IStringLocalizer<ResX_Errors> resxCoreErrors, ILogger<HttpSendHandlerBase<TResponse>> logger)
    {
        _httpClientFactory = httpClientFactory;
        _resxCoreErrors = resxCoreErrors;
        _logger = logger;
    }

    protected async Task<TResponse> Handle(HttpSendCommandBase request, CancellationToken cancellationToken)
    {
        var httpClient = request.HttpClientType switch
        {
            HttpClientTypeEnum.Authorized => await _httpClientFactory.CreateAuthClientAsync(),
            HttpClientTypeEnum.NonAuthorized => await _httpClientFactory.CreateNonAuthClientAsync(),
            _ => throw new ArgumentOutOfRangeException()
        };

        return request.Method switch
        {
            ApiMethod.Get => await CallHttp(httpClient, $"{request.Url}{GetQueryString(request)}", ApiMethod.Get, request.ApiRequest, request.Type),
            ApiMethod.Post => await CallHttp(httpClient, request.Url, ApiMethod.Post, request.ApiRequest, request.Type),
            ApiMethod.Put => await CallHttp(httpClient, request.Url, ApiMethod.Put, request.ApiRequest, request.Type),
            ApiMethod.Delete => await CallHttp(httpClient, $"{request.Url}{GetQueryString(request)}", ApiMethod.Delete, request.ApiRequest, request.Type),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static string GetQueryString(HttpSendCommandBase request)
    {
        var nameValueCollection = new NameValueCollection();

        foreach (var propertyInfo in request.ApiRequest.GetType()
                     .GetProperties(
                         BindingFlags.Public
                         | BindingFlags.Instance))
        {
            var val = propertyInfo.GetValue(request.ApiRequest, null);
            if (val == null)
                continue;
            nameValueCollection.Add(propertyInfo.Name.ToLower(), val.ToString());
        }

        return ToQueryString(nameValueCollection);
    }

    private async Task<TResponse> CallHttp(HttpClient httpClient, string servicePath, ApiMethod method, ApiRequestBase? data = null, Type? type = null)
    {
        TResponse apiResponse = new();
        apiResponse.Time = DateTime.UtcNow;
        JsonContent? jsonContent = null;
        if (data != null && type != null)
        {
            jsonContent = JsonContent.Create(data, type);
        }

        var response = method switch
        {
            ApiMethod.Get => await httpClient.GetAsync(servicePath),
            ApiMethod.Post => await httpClient.PostAsync(servicePath, jsonContent),
            ApiMethod.Put => await httpClient.PutAsync(servicePath, jsonContent),
            ApiMethod.Delete => await httpClient.DeleteAsync(servicePath),

            _ => throw new Exception("Api client is not created.")
        };

        if (response == null)
            throw new NullReferenceException($"HttpClient returned null {nameof(HttpResponseMessage)}.");

        apiResponse.StatusCode = response.StatusCode;
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var resp = await response.Content.ReadFromJsonAsync<TResponse>();
                if (resp == null)
                {
                    apiResponse.Exception = new Exception("API response returned null.");
                    apiResponse.Code = ApiResponseBase.Code_ErrorParseJson;
                    apiResponse.ShortMessage = _resxCoreErrors[ResX_Errors.ApiResponseBaseStatusCode_ERROR_PARSEJSON];
                    apiResponse.Message = apiResponse.Exception.MessageRecur();
                    return apiResponse;
                }
                apiResponse = resp; 
                apiResponse.StatusCode = HttpStatusCode.OK;
                apiResponse.Time = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                apiResponse.Exception = ex;
                apiResponse.Code = ApiResponseBase.Code_ErrorParseJson;
                apiResponse.ShortMessage = _resxCoreErrors[ResX_Errors.ApiResponseBaseStatusCode_ERROR_PARSEJSON];
                apiResponse.Message = ex.MessageRecur();

            }

            return apiResponse;
        }


        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                apiResponse.Code = ApiResponseBase.Code_ErrorBadRequest;
                apiResponse.ShortMessage = _resxCoreErrors[ResX_Errors.ApiResponseBaseStatusCode_ERROR_BADREQUEST_RESPONSE];
                break;
            case HttpStatusCode.InternalServerError:
                apiResponse.Code = ApiResponseBase.Code_ErrorInternalServer;
                apiResponse.ShortMessage = _resxCoreErrors[ResX_Errors.ApiResponseBaseStatusCode_ERROR_INTERNALSERVERERROR_RESPONSE];
                break;
            case HttpStatusCode.Unauthorized:
                apiResponse.Code = ApiResponseBase.Code_ErrorUnauthorizedResponse;
                apiResponse.ShortMessage = _resxCoreErrors[ResX_Errors.ApiResponseBaseStatusCode_ERROR_UNAUTHORIZED_RESPONSE];
                break;
            default:
                apiResponse.Code = ApiResponseBase.Code_ErrorOtherResponse;
                apiResponse.ShortMessage = _resxCoreErrors[ResX_Errors.ApiResponseBaseStatusCode_ERROR_OTHER_RESPONSE];
                break;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.InternalServerError:
            default:
                var json = string.Empty;
                if (jsonContent != null)
                    json = await jsonContent.ReadAsStringAsync();
                _logger.LogError("{Method} thrown error. Url:{request};Data:{data}", nameof(CallHttp), servicePath, json);
                break;
        }

        apiResponse.Message = apiResponse.ShortMessage;

        return apiResponse;
    }

    private static string ToQueryString(NameValueCollection nvc)
    {
        var sb = new StringBuilder();

        foreach (string key in nvc.Keys)
        {
            if (string.IsNullOrWhiteSpace(key)) continue;

            var values = nvc.GetValues(key);
            if (values == null)
                continue;

            foreach (var value in values)
            {
                sb.Append(sb.Length == 0 ? "?" : "&");
                sb.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
            }
        }

        return sb.ToString();
    }
}