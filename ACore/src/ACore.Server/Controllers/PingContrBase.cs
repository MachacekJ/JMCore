//using System.Threading.Tasks;
//using JMCore.Models.BaseRR;
//using JMCore.Server.Models.BaseRR;
//using Microsoft.Extensions.Logging;

//namespace JMCore.Server.Controllers;

///// <summary>
///// PP
///// </summary>
//public class PingControllerBase : ControllerBase
//{
//    /// <summary>
//    /// Ctor.
//    /// </summary>
//    public PingControllerBase(ILogger<PingControllerBase> logger) : base(logger)
//    {
//    }

//    /// <summary>
//    /// PP
//    /// </summary>
//    /// <param name="request"></param>
//    /// <returns></returns>
//    public async Task<ApiResponseBase<ApiResponseBaseDto>> PingBase(RequestBaseDto request)
//    {
//        var res = new ApiResponseBase<ApiResponseBaseDto>();
//        res.Ok(this, new ApiResponseBaseDto
//        {
//            Message = "Pong"
//        });
//        await Task.Delay(0);
//        return res;
//    }
//}