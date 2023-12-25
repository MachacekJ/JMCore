using JMCore.Services.JMCache;

namespace JMCore.Tests.CoreT.ServicesT.JMCacheT
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