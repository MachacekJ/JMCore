using JMCore.Server.CQRS.DB.BasicStructure.SettingGet;
using JMCore.Server.DB.Models;
using Microsoft.EntityFrameworkCore;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.AuditStructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.DB.Abstract;

public abstract class DbContextBase : DbContext, IDbContextBase
{
    public const string DbContextVersionKeyPrefix = "DBContextVersion_";

    public abstract DbScriptBase SqlScripts { get; }
    public abstract string DbContextName { get; }

    private readonly IAuditDbService? _auditService;
    private readonly DbContextBaseHelper _dbContextHelper;
    protected readonly ILogger<DbContextBase> Logger;
    protected IMediator Mediator { get; }

    private string DbContextName2 => DbContextName;
    private DbScriptBase SqlScripts2 => SqlScripts;

    protected DbContextBase(DbContextOptions options, IMediator mediator, ILogger<DbContextBase> logger, IAuditDbService? auditService = null) :
        base(options)
    {
        Logger = logger ?? throw new ArgumentException($"{nameof(logger)} is null.");
        Mediator = mediator ?? throw new ArgumentException($"{nameof(mediator)} is null.");

        _auditService = auditService;
        _dbContextHelper = new DbContextBaseHelper(this, Mediator, Logger, DbContextName2, SqlScripts2);
    }


    #region Audit

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        Logger.LogError($"Don't use {nameof(SaveChanges)} in EF. Use {nameof(SaveChangesAsync)}.");
        if (_auditService == null || !IsAuditEnabledAsync().Result)
        {
            return SaveChangesAsync(CancellationToken.None).Result;
        }
        
        var entityAudits = _auditService.OnBeforeSaveChangesAsync(ChangeTracker).Result;
        var result = base.SaveChanges(acceptAllChangesOnSuccess);
        _auditService.OnAfterSaveChangesAsync(entityAudits).Wait();
        return result;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        if (_auditService == null || !(await IsAuditEnabledAsync()))
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        var entityAudits = await _auditService.OnBeforeSaveChangesAsync(ChangeTracker);
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await _auditService.OnAfterSaveChangesAsync(entityAudits);

        return result;
    }

    #endregion

    public async Task<UpdateDbResponse> UpdateDbAsync()
    {
        return await _dbContextHelper.UpdateDbAsync(AfterUpdateAsync);
    }

    protected virtual Task AfterUpdateAsync()
    {
        return Task.CompletedTask;
    }

    private async Task<bool> IsAuditEnabledAsync()
    {
        if (_auditService == null)
            return false;

        var isAuditTable = await Mediator.Send(new SettingGetQuery(DbContextVersionKeyPrefix + nameof(AuditDbContext)));

        return !string.IsNullOrEmpty(isAuditTable);
    }
}