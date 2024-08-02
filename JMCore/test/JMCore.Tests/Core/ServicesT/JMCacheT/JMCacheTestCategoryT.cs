using JMCore.Modules.CacheModule;

namespace JMCore.Tests.Core.ServicesT.JMCacheT
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class JMCacheTestCategoryT : JMCacheCategory
    {
        public const int TestCache = 200;

        public JMCacheTestCategoryT()
        {
            AddCategory(TestCache, "TestCache");
        }
    }
}