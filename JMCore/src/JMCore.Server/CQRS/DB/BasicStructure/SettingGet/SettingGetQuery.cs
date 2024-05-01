using JMCore.Server.Storages.Abstract;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingGet
{
    public class SettingGetQuery : IDbRequest<string?>
    {
        public SettingGetQuery(string key, bool isRequired = false)
        {
            Key = key;
            IsRequired = isRequired;
        }

        public string Key { get; }
        public bool IsRequired { get; }
    }
}
