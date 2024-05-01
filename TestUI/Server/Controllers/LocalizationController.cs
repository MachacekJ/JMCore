using JMCore.Localizer;
using JMCore.Server.Controllers;
using JMCore.Server.Storages.DbContexts.LocalizeStructure;
using JMCoreTest.Blazor.Shared.Controllers.LocalizationController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JMCoreTest.Blazor.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocalizationController : BaseController<LocalizationController>
{
    private readonly ILogger<LocalizationController> _logger;
    private readonly ILocalizeDbContext _db;

    public LocalizationController(ILogger<LocalizationController> logger, ILocalizeDbContext db) : base(logger)
    {
        _logger = logger;
        _db = db;
    }

    [AllowAnonymous]
    [HttpGet("ClientLanguagePack")]
    public async Task<ClientLanguagePackAsyncResponse> ClientLanguagePackAsync(int lcid, DateTime? lastSyncDateTime)
    {
        var res = new ClientLanguagePackAsyncResponse();
        await RunInCatch<ClientLanguagePackAsyncResponse>(res,
            async () =>
            {
                res.Data = (await _db.ClientLocalizations(lcid, lastSyncDateTime)).Select(i => new LocalizationRecord(i.Id, i.MsgId, i.Translation, i.Lcid, i.ContextId, i.Scope, i.Changed));
            });
        return res;
    }
}