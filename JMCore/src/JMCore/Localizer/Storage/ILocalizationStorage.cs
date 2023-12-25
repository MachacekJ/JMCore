namespace JMCore.Localizer.Storage;

/// <summary>
/// Interface for implementation localization storage.
/// </summary>
public interface ILocalizationStorage
{
    IEnumerable<ILocalizationRecord> All { get; }
    
    void PopulateLocalizationStorage(IEnumerable<ILocalizationRecord> localizationRecords);
}