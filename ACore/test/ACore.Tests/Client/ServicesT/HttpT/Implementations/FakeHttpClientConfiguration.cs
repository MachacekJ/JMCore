// using System.Net;
// using System.Reflection;
//
// namespace ACore.Tests.Client.ServicesT.HttpT.Implementations;
//
// public class FakeHttpClientConfiguration
// {
//     /// <summary>
//     /// The relative path in the binary to the response json file as the requests go.
//     /// </summary>
//     public string ResponseJson { get; private set; }
//
//     public string CallingUrl { get; }
//     public HttpStatusCode StatusCode { get; }
//
//     /// <summary>
//     /// SendAction callback returns <see cref="HttpRequestMessage"/>.
//     /// </summary>
//     public Action<HttpRequestMessage, HttpResponseMessage>? SendActionCallback { get; }
//
//     public FakeHttpClientConfiguration(string responseJson, string callingUrl, HttpStatusCode statusCode = HttpStatusCode.OK, Action<HttpRequestMessage, HttpResponseMessage>? sendActionCallback = null)
//     {
//         ResponseJson = responseJson;
//         CallingUrl = callingUrl;
//         StatusCode = statusCode;
//         SendActionCallback = sendActionCallback;
//     }
//
//     public void LoadJsonFile(string relativePath)
//     {
//         ResponseJson = ReadFile(relativePath);
//     }
//
//     private static string ReadFile(string fileName)
//     {
//         var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
//             fileName);
//         return File.ReadAllText(path);
//     }
// }