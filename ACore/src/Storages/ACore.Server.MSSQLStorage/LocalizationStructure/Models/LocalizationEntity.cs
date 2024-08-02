using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Localizer;

namespace ACore.Server.MSSQLStorage.LocalizationStructure.Models;

[Table("Localization")]
public class LocalizationEntity : ILocalizationRecord
{
    [Key]
    public int Id { get; set; }

    [MaxLength(255)] public string MsgId { get; set; } = null!;


    public string Translation { get; set; } = null!;

    public int Lcid { get; set; }

    public string ContextId { get; set; } = null!;

    public LocalizationScopeEnum Scope { get; set; }

    public DateTime? Changed { get; set; }
    public void SetTranslation(string newTranslation)
    {
        Translation = newTranslation;
    }
}