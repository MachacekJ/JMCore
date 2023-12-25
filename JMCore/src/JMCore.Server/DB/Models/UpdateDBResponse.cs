using JMCore.Models.BaseRR;

namespace JMCore.Server.DB.Models
{
    public class UpdateDbResponse : ResponseBase
    {
        public string NewVersion { get; set; } = null!;
    }
}
