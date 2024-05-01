using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.BasicStructure.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Server.DataStorages.PG.BasicStructure.Models
{
    [Auditable]
    [Table("setting")]
    public class SettingEntity: ISettingEntity
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
