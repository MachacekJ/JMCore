using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.BasicStructure.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Server.DataStorages.Memory.BasicStructure
{
    [Auditable]
    public class SettingEntity: ISettingEntity
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool? IsSystem { get; set; }
    }
}
