namespace JMCore.Services.JMCache
{
    public class JMCacheCategory : IJMCacheCategories
    {
        public Dictionary<int, string> All { get; } = new();
        
        /// <summary>
        /// Add new category to cache.
        /// </summary>
        protected void AddCategory(int idCategory, string category)
        {
            if (All.ContainsKey(idCategory))
                throw new Exception("Cache - idCategory: " + idCategory + " is already exists.");

            All.Add(idCategory, category);
        }
    }
}
