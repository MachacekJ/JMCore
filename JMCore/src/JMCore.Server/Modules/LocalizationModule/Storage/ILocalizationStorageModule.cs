using JMCore.Localizer;
using JMCore.Server.Modules.LocalizationModule.Storage.Models;

namespace JMCore.Server.Modules.LocalizationModule.Storage;

public interface ILocalizationStorageModule
{
  /// <summary>
  /// If ChangeDate cannot be null, default value from resX cannot be overwritten.
  /// </summary>
  /// <returns>Translated text</returns>
  Task<string> SyncResXItemAsync(ILocalizationRecord loc);


  /// <summary>
  /// Pastes new translation to db for item. Rewrites default translation from ResX.
  /// </summary>
  Task ChangeTranslationAsync(int idEntity, string newTranslation);

  /// <summary>
  /// Return all client translations <see cref="LocalizationEntity"/> for specific language.
  /// </summary>
  Task<List<LocalizationEntity>> ClientLocalizations(int lcid, DateTime? lastSync);
}