using JMCore.Localizer;
using JMCore.Localizer.Storage;
using JMCore.Server.Localizer;
using JMCore.Server.ResX;
using JMCore.Server.Storages.Abstract;
using JMCore.Server.Storages.DbContexts.LocalizeStructure.Models;
using JMCore.Server.Storages.DbContexts.LocalizeStructure.Scripts.Postgres;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace JMCore.Server.Storages.DbContexts.LocalizeStructure;

public class LocalizeDbContext : DbContextBase, ILocalizeDbContext
{
    private readonly IOptions<ResXLocalizationOptions> _resxOptions;
    private readonly ILocalizationStorage _localizationProvider;

    public DbSet<LocalizationEntity> Localizations { get; set; }

    private readonly ScriptRegistrations _dbSqlScript = new();

    public override DbScriptBase SqlScripts => _dbSqlScript;
    public override string DbContextName => GetType().Name;

    public LocalizeDbContext(DbContextOptions<LocalizeDbContext> options, IMediator mediator, IOptions<ResXLocalizationOptions> resxOptions, ILocalizationStorage localizationProvider, ILogger<LocalizeDbContext> logger) : base(options, mediator, logger)
    {
        _resxOptions = resxOptions;
        _localizationProvider = localizationProvider;
    }

    #region Localization Table

    public async Task<string> SyncResXItemAsync(ILocalizationRecord loc)
    {
        var item = await Localizations.SingleOrDefaultAsync(a =>
            a.MsgId == loc.MsgId &&
            a.ContextId == loc.ContextId &&
            a.Lcid == loc.Lcid);

        if (item == null)
        {
            item = new LocalizationEntity();
            Localizations.Add(item);
        }
        else
        {
            if (item.Changed != null)
                return item.Translation;
        }

        // Not rewrite custom db translation.
        if ((item.Translation != loc.Translation || item.Scope != loc.Scope) && item.Changed == null)
        {
            item.Translation = loc.Translation;
            item.MsgId = loc.MsgId;
            item.ContextId = loc.ContextId;
            item.Scope = loc.Scope;
            item.Lcid = loc.Lcid;
            item.Changed = loc.Changed;
            await SaveChangesAsync();
        }

        loc.Id = item.Id;
        return item.Translation;
    }

    public async Task ChangeTranslationAsync(int idEntity, string newTranslation)
    {
        var item = await Localizations.SingleAsync(a => a.Id == idEntity);
        item.Changed = DateTime.UtcNow;
        item.Translation = newTranslation;
        await SaveChangesAsync();

        await LocalizerExtensions.LoadLocalizationStorageAsync(_resxOptions, this, _localizationProvider);
    }

    public async Task<List<LocalizationEntity>> ClientLocalizations(int lcid, DateTime? lastSync)
    {
        if (lastSync == null)
            return await Localizations.Where(l => l.Scope.HasFlag(LocalizationScopeEnum.Client)).ToListAsync();

        return await Localizations.Where(l => l.Scope.HasFlag(LocalizationScopeEnum.Client) && l.Changed != null && l.Changed > lastSync).ToListAsync();
    }

    #endregion
}