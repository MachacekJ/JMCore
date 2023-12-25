using System.Net;

namespace JMCore.Models.BaseRR;

public class ApiResponseBase : ResponseBase
{
    public static readonly int Code_ErrorBadRequest = -400;
    public static readonly int Code_ErrorParseJson = -450;
    public static readonly int Code_ErrorUnauthorizedResponse = -401;
    public static readonly int Code_ErrorOtherResponse = -499;
    public static readonly int Code_ErrorInternalServer = -500;
    
    public Guid? ServerErrorId { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    
    public ApiResponseBase()
    {
        AllStatus.Add(Code_ErrorBadRequest, nameof(Code_ErrorBadRequest));
        AllStatus.Add(Code_ErrorParseJson, nameof(Code_ErrorParseJson));
        AllStatus.Add(Code_ErrorUnauthorizedResponse, nameof(Code_ErrorUnauthorizedResponse));
        AllStatus.Add(Code_ErrorInternalServer, nameof(Code_ErrorInternalServer));
        AllStatus.Add(Code_ErrorOtherResponse, nameof(Code_ErrorOtherResponse));
    }
}