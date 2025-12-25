namespace GlideBuy.Core.Caching
{
	/// <summary>
	/// An interface for a cache key registry that keeps track of all cache keys
	/// currently in use by the application.
	/// 
	/// This allows us to:
	/// Invalidate specific cache entries by name or prefix.
	/// Clear the entire cache when necessary.
	/// Inspect all active keys (for debugging, or for selective cache reset after data changes).
	/// 
	/// Cache invalidation must sometimes be targeted. For example, when a product
	/// is updated, all cache entries starting with "Store.product." should be cleared.
	/// But we don’t want to clear everything, since that would hurt performance.
	/// </summary>
	public interface ICacheKeyManager
	{
		void AddKey(string key);

		void RemoveKey(string key);

		void Clear(string key);

		IEnumerable<string> RemoveByPrefix(string prefix);

		IEnumerable<string> Keys { get; }
	}
}
