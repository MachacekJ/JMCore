﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.DB.Audit;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Server.DB.DbContexts.BasicStructure.Models
{
    [Auditable]
    [Table("Setting")]
    public class SettingEntity
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public bool? IsSystem { get; set; }
    }
}
