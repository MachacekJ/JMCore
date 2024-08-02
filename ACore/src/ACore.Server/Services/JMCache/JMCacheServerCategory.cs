using ACore.Modules.CacheModule;

namespace ACore.Server.Services.JMCache
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
