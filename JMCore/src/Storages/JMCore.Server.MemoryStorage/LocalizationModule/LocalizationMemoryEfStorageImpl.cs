using JMCore.Localizer.Storage;
using JMCore.Server.Modules.LocalizationModule.Storage.EF;
using JMCore.Server.ResX;
using JMCore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JMCore.Server.MemoryStorage.LocalizationModule;

public class LocalizationMemoryEfStorageImpl(DbContextOptions<LocalizationMemoryEfStorageImpl> options, IMediator mediator, IOptions<ResXLocalizationOptions> resxOptions, ILocalizationStorage localizationProvider, ILogger<LocalizationStorageEfContext> logger)
  : LocalizationStorageEfContext(options, mediator, resxOptions, localizationProvider, logger)
{
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
}