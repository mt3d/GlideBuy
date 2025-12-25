namespace GlideBuy.Core.Caching
{
	/// <summary>
	/// A cache key service interface.
	/// </summary>
	public interface ICacheKeyBuilder
	{
		/**
		 * Takes an initial CacheKey object (which defines a base key string, e.g. "language.all")
		 * Combines it with parameters (like storeId, showHidden, etc.)
		 * Returns a new CacheKey instance that includes those parameters.
		 */
		CacheKey BuildKey(CacheKey key, params object[] cacheKeyParams);

		/**
		 * Same as above, but assign the default cache duration.
		 */
		CacheKey BuildKeyWithDefaultCacheTime(CacheKey key, params object[] cacheKeyParams);
	}
}
