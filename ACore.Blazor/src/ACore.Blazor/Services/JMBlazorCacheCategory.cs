using ACore.Modules.CacheModule;

namespace ACore.Blazor.Services
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