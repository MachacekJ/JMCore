using JMCore.Modules.CacheModule;

namespace JMCore.Blazor.Services
{
    public class JMBlazorCacheCategory : JMCacheCategory
    {
        public const int LogCache = 200;

        public JMBlazorCacheCategory()
        {
            AddCategory(LogCache, "Log");
        }
    }
}