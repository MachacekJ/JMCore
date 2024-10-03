// using ACore.Client.Services.Http;
// using ACore.Models.BaseRR;
//
// namespace ACore.Client.CQRS.Http;
//
// public class HttpSendCommandBase
// {
//     public HttpClientTypeEnum HttpClientType { get; }
//     public ApiRequestBase ApiRequest { get; }
//     public ApiMethod Method { get; }
//
//     public string Url { get; }
//
//     public Type Type { get; }
//
//
//     protected HttpSendCommandBase(HttpClientTypeEnum httpClientType, ApiMethod method, string url, ApiRequestBase apiRequest, Type type)
//     {
//         HttpClientType = httpClientType;
//         ApiRequest = apiRequest;
//         Method = method;
//         Url = url;
//         Type = type;
//     }
// }