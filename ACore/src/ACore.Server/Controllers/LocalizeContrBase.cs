//using JMCore.Models;
//using JMCore.Models.BaseRR;
//using JMCore.Server.DB.LocalizeStructure;
//using JMCore.Server.Models.BaseRR;
//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;
//using JMCore.Resources;

//namespace JMCore.Server.Controllers;

//public class LocalizeControllerBase : ControllerBase
//{
//    private readonly ILocalizeDbContext _context;
//    public LocalizeControllerBase(ILogger<LocalizeControllerBase> logger, ILocalizeDbContext localizeDb) : base(logger)
//    {
//        _context = localizeDb;
//    }
    
//    public async Task<ApiResponseBase<LocalizeApiResponseDto>> AllStrings(RequestBaseDto req)
//    {
//        var res = new ApiResponseBase<LocalizeApiResponseDto>();

//        await RunInCatch(res, async () =>
//        {
//            res.DTO.Data = await _context.AllAsync(LocalizationScopeEnum.Client);
//        });

//        return res;
//    }
//}

