using JMCore.Modules.CacheModule;

namespace JMCore.Server.Services.JMCache
{
    public class JMCacheServerCategory : JMCacheCategory
    {
        public const int DbTable = 200;
        public JMCacheServerCategory()
        {
            AddCategory(DbTable, "DBTable");
        }
    }
}
