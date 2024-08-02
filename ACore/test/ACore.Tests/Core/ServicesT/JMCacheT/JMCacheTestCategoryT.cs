using ACore.Modules.CacheModule;

namespace ACore.Tests.Core.ServicesT.JMCacheT
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