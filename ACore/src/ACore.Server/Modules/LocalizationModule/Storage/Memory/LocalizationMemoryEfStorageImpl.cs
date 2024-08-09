using ACore.Localizer.Storage;
using ACore.Server.Modules.LocalizationModule.ResX;
using ACore.Server.Modules.LocalizationModule.Storage.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.LocalizationModule.Storage.Memory;

public class LocalizationMemoryEfStorageImpl(DbContextOptions<LocalizationMemoryEfStorageImpl> options, IMediator mediator, IOptions<ResXLocalizationOptions> resxOptions, ILocalizationStorage localizationProvider, ILogger<LocalizationStorageEfContext> logger)
  : LocalizationStorageEfContext(options, mediator, resxOptions, localizationProvider, logger)
{
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
}