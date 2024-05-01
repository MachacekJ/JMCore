using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Audit;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Server.Storages.DbContexts.BasicStructure.Models
{
    [Auditable]
    [Table("setting")]
    public class SettingEntity
    {
        [Key]
        [Column("setting_id")]
        public int Id { get; set; }

        [Column("key")]
        public string Key { get; set; } = null!;
       
        [Column("value")]
        public string Value { get; set; } = null!;
        
        [Column("is_system")]
        public bool? IsSystem { get; set; }
    }
}
