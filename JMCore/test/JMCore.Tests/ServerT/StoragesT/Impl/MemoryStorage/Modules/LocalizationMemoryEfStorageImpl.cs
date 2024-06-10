using JMCore.Localizer.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.ResX;
using JMCore.Server.Storages.Modules.LocalizationModule.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;

public class LocalizationMemoryEfStorageImpl(DbContextOptions<LocalizationMemoryEfStorageImpl> options, IMediator mediator, IOptions<ResXLocalizationOptions> resxOptions, ILocalizationStorage localizationProvider, ILogger<LocalizationStorageEfContext> logger)
  : LocalizationStorageEfContext(options, mediator, resxOptions, localizationProvider, logger)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;
}