using JMCore.Services.JMCache;

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
