using ACore.Localizer;
using ACore.Localizer.Storage;
using ACore.Server.Modules.LocalizationModule.Configuration;
using ACore.Server.Modules.LocalizationModule.Storage.Models;
using ACore.Server.ResX;
using ACore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.LocalizationModule.Storage.EF;
// ReSharper disable once UnusedAutoPropertyAccessor.Global

public abstract class LocalizationStorageEfContext(DbContextOptions options, IMediator mediator, IOptions<ResXLocalizationOptions> resxOptions, ILocalizationStorage localizationProvider, ILogger<LocalizationStorageEfContext> logger)
  : DbContextBase(options, mediator, logger), ILocalizationStorageModule
{

  public DbSet<LocalizationEntity> Localizations { get; set; }

  private readonly ScriptRegistrations _dbSqlScript = new();

  public override DbScriptBase UpdateScripts => _dbSqlScript;

  protected override string ModuleName => nameof(ILocalizationStorageModule);
  public override Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class
  {
    throw new NotImplementedException();
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

    await LocalizerExtensions.LoadLocalizationStorageAsync(resxOptions, this, localizationProvider);
  }

  public async Task<List<LocalizationEntity>> ClientLocalizations(int lcid, DateTime? lastSync)
  {
    if (lastSync == null)
      return await Localizations.Where(l => l.Scope.HasFlag(LocalizationScopeEnum.Client)).ToListAsync();

    return await Localizations.Where(l => l.Scope.HasFlag(LocalizationScopeEnum.Client) && l.Changed != null && l.Changed > lastSync).ToListAsync();
  }

  #endregion
}