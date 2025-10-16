namespace GlideBuy.Core.Caching
{
	/**
	 * Caching works by saving data under a unique key. If you always use the same
	 * key string, you'll always get the same cached value.
	 * 
	 * When the cached data depends on parameters, the cache key should reflect
	 * those parameters. For instance, in a multi-store setting, the cache key
	 * should reflect the store id.
	 * 
	 * A cache key is smarter than a simple string — it helps with:
	 * Cache expiry
	 * Group invalidation (e.g., remove all keys with prefix "languages".)
	 * Clear naming conventions
	 */
	public class CacheKey
	{
		public CacheKey(string key)
		{
			Key = key;
		}

		/// <summary>
		/// If key parameters are supplied, then it is assumed that the key string
		/// contain placeholders
		/// </summary>
		/// <param name="addCacheKeyParams"></param>
		/// <param name="keyObjects"></param>
		/// <returns></returns>
		public CacheKey Create(Func<object, object> createCacheKeyParams, params object[] keyObjects)
		{
			CacheKey cacheKey = new (Key);

			if (!keyObjects.Any())
			{
				return cacheKey;
			}

			// Fill in the placeholders. Each key is process by createCacheKeyParams before
			// being inserted.
			cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParams).ToArray());

			return cacheKey;
		}

		public string Key { get; protected set; }

		// TODO: Use AppSettings.
		/// <summary>
		/// Gets or sets cach time in minutes.
		/// </summary>
		public int CacheTimeMinute { get; set; } = 60; // Singleton<AppSettings>.Instance.Get<CacheConfig>().DefaultCacheTimeMinutes;
	}
}
