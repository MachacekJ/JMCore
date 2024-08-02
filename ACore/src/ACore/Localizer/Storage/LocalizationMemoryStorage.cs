namespace ACore.Localizer.Storage;

public class LocalizationMemoryStorage : ILocalizationStorage
{
    private IEnumerable<ILocalizationRecord> _all = new List<ILocalizationRecord>();

    public IEnumerable<ILocalizationRecord> All => _all;

    public void PopulateLocalizationStorage(IEnumerable<ILocalizationRecord> localizationRecords)
    {
        _all = localizationRecords;
    }
}