using System.Collections.Concurrent;

namespace GlideBuy.Core.Caching
{
	/// <summary>
	/// A central index of all cache entries managed by nopCommerce’s caching layer.
	/// 
	/// This class should be registered as a singleton instance, since all
	/// caching operations share the same key manager.
	/// 
	/// The underlying IConcurrentCollection is thread-safe, allowing concurrent
	/// reads/writes from multiple requests. So multiple threads can add or remove
	/// cache keys simultaneously without race conditions.
	/// </summary>
	public class CacheKeyManager : ICacheKeyManager
	{
		/**
		 * TODO: Replace with a radix tree.A radix tree(prefix tree / trie) is
		 * perfect for prefix-based lookups, which caching systems often need for
		 * efficient invalidation.
		 * 
		 * If we intend to remove keys by prefix, then a radix tree allows this to
		 * happen in O(P) time, where P = length of prefix — much faster than scanning
		 * every key in a dictionary or set. So instead of linear scanning(O(N)), the
		 * trie jumps directly to the “product:” branch and removes everything under it.
		 * 
		 * We only cares about the key path, not the stored value.
		 * 
		 * ConcurrentBag<string> is not suitable for key tracking.
		 * They don’t support efficient lookup or removal by key.
		 * You’d have to rebuild the bag/queue every time you remove by prefix.
		 * Could be used if we  just want to keep a running log of added keys, not
		 * for removals or queries.
		*/
		protected readonly ConcurrentDictionary<string, byte> keys;

		public IEnumerable<string> Keys => keys.Keys;

		public void AddKey(string key)
		{
			keys.TryAdd(key, default);
		}

		public void Clear(string key)
		{
			keys.Clear();
		}

		// TODO: Replace implementation when 
		// RemoveByPrefix is O(N) because it must scan all keys.
		public IEnumerable<string> RemoveByPrefix(string prefix)
		{
			var toRemove = keys.Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

			foreach (var key in toRemove)
			{
				keys.TryRemove(key, out _);
			}

			return toRemove;
		}

		public void RemoveKey(string key)
		{
			keys.TryRemove(key, out _);
		}
	}
}
