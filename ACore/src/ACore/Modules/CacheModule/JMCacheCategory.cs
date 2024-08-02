namespace ACore.Modules.CacheModule
{
    public class JMCacheCategory : IJMCacheCategories
    {
        public Dictionary<int, string> All { get; } = new();
        
        /// <summary>
        /// Add new category to cache.
        /// </summary>
        protected void AddCategory(int idCategory, string category)
        {
            if (!All.TryAdd(idCategory, category))
                throw new Exception("Cache - idCategory: " + idCategory + " is already exists.");
        }
    }
}
