using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Localizer;

namespace JMCore.Server.Storages.Modules.LocalizationModule.Models;

[Table("localization")]
public class LocalizationEntity : ILocalizationRecord
{
  [Key]
  [Column("localization_id")]
  public int Id { get; set; }

  [Column("msgid")]
  [MaxLength(255)]
  public string MsgId { get; set; } = null!;

  [Column("translation")]
  public string Translation { get; set; } = null!;

  [Column("lcid")]
  public int Lcid { get; set; }

  [Column("contextid")]
  public string ContextId { get; set; } = null!;

  [Column("scope")]
  public LocalizationScopeEnum Scope { get; set; }

  [Column("changed")]
  public DateTime? Changed { get; set; }

  public void SetTranslation(string newTranslation)
  {
    Translation = newTranslation;
  }
}