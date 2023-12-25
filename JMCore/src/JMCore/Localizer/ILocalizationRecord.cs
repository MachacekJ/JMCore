namespace JMCore.Localizer;

public interface ILocalizationRecord
{
    public int Id { get; set; }

    public string MsgId { get; }

    public string Translation { get; }

    public int Lcid { get; }

    public string ContextId { get; }
    
    public LocalizationScopeEnum Scope { get; }
    
    public DateTime? Changed { get; }

    public void SetTranslation(string newTranslation);
}