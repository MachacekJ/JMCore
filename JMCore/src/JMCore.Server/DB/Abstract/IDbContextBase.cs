using JMCore.Server.DB.Models;

namespace JMCore.Server.DB.Abstract
{
    public interface IDbContextBase
    {
        Task<UpdateDbResponse> UpdateDbAsync();
        DbScriptBase SqlScripts { get; }
        string DbContextName { get; }
    }
}
