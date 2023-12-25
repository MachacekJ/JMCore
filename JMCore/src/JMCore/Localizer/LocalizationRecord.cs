namespace JMCore.Localizer;

public class LocalizationRecord : ILocalizationRecord
{
    public LocalizationRecord(int id, string msgId, string translation, int lcid, string contextId, LocalizationScopeEnum scope, DateTime? changed)
    {
        Id = id;
        MsgId = msgId;
        Translation = translation;
        Lcid = lcid;
        ContextId = contextId;
        Scope = scope;
        Changed = changed;
    }

    public int Id { get; set; }
    public string MsgId { get; }
    public string Translation { get; private set; }
    public int Lcid { get; }
    public string ContextId { get; }
    public LocalizationScopeEnum Scope { get; }
    public DateTime? Changed { get; }
    public void SetTranslation(string newTranslation)
    {
        Translation = newTranslation;
    }
}