using JMCore.Localizer.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.ResX;
using JMCore.Server.Storages.Modules.LocalizeModule.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;

public class LocalizationEfStorageImpl(DbContextOptions<LocalizationEfStorageImpl> options, IMediator mediator, IOptions<ResXLocalizationOptions> resxOptions, ILocalizationStorage localizationProvider, ILogger<LocalizeStorageEfContext> logger)
  : LocalizeStorageEfContext(options, mediator, resxOptions, localizationProvider, logger)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;
}