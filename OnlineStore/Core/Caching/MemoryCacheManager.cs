
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GlideBuy.Core.Caching
{
	public class MemoryCacheManager : CacheKeyBuilder, IStaticCacheManager
	{
		protected readonly IMemoryCache memoryCache;
		protected readonly ICacheKeyManager cacheKeyManager;

		public MemoryCacheManager(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Rretrieves a cached value of type T if available; otherwise, runs the
		/// provided acquire() function to load it, stores the result in cache,
		/// and then returns it.
		/// 
		/// It's an "asynchronous lazy cache loader":
		/// The first caller triggers the loading.
		/// Other concurrent callers wait for the same task, rather than triggering duplicate loads.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="acquire"></param>
		/// <returns></returns>
		public async Task<T> GetAsync<T>(CacheKey? key, Func<Task<T>> acquire)
		{
			/**
			 * This is a cache-aside + Lazy<Task> hybrid — it solves common concurrency
			 * and consistency problems:
			 * 
			 * Problem => Solution
			 * Multiple requests cause multiple cache loads	=> Lazy<Task<T>> ensures only one load happens
			 * Cache entry created before async completes => Lazy defers it until Value is awaited (see below)
			 * Exceptions or nulls stuck in cache => Explicitly removes entry on error or null
			 * Disable cache when desired => CacheTime <= 0 shortcut
			 */

			// If the cache time is <= 0, then treat it as "don't cache".
			if ((key?.CacheTimeMinute ?? 0) <= 0)
			{
				return await acquire();
			}

			/**
			 * When you call something like:
			 * _memoryCache.GetOrCreate("key", entry => SomeAsyncOperation());
			 * 
			 * this means:
			 * GetOrCreate immediately stores the return value of the factory (SomeAsyncOperation()).
			 * But SomeAsyncOperation() starts running right away — even before anyone awaits it.
			 * So the cache entry now contains a Task that is already in progress.
			 * 
			 * but it has a pitfall:
			 * If the async operation takes time and multiple threads call GetOrCreate() concurrently
			 * before the cache is populated, you can get multiple concurrent loads, because
			 * the cache entry doesn’t exist until the first factory completes.
			 * 
			 * With Lazy<T>:
			 * The cache entry is created immediately, and the Lazy<Task<T>> object is stored right away.
			 * The actual async operation (acquire()) is not started yet.
			 * 
			 * Normally, an async operation must complete before the cache entry exists.
			 * With Lazy<Task<T>>, the cache entry exists immediately (the Lazy object itself),
			 * but the actual operation inside it hasn’t run yet — it’s deferred until .Value is accessed.
			 * 
			 * Thread A: Calls GetOrCreate("key")
			 * Thread B: Calls GetOrCreate("key")
			 * A: Cache doesn’t have key → creates new Lazy<Task<T>>
			 * B: Finds the same Lazy<Task<T>> in cache
			 * A: Thread A calls .Value → starts acquire()
			 * B: Thread B also calls .Value → waits for same task
			 * Only one async load happens
			 */
			var task = memoryCache.GetOrCreate(
				key.Key,
				entry =>
				{
					//entry.SetOptions(PrepareEntryOptions);

					/**
					 * The actual data (Task<T>) isn’t fetched immediately.
					 * It’s wrapped in a Lazy<Task<T>> that executes acquire() only once,
					 * even if multiple threads request the same key concurrently.
					 * 
					 * So if three requests come in at the same time:
					 * The first creates the entry and starts acquire().
					 * The other two wait for the same Task<T> to complete.
					 * 
					 * new Lazy<Task<T>>(acquire, true) inside a cache factory looks odd because
					 * it’s "lazy over async". But it’s actually a well-known advanced pattern used in:
					 * ASP.NET Core caching
					 * Polly caching extensions
					 * StackOverflow’s caching libraries
					 * 
					 * It elegantly avoids race conditions without needing locks or semaphores.
					 */
					return new Lazy<Task<T>>(acquire, true);
				});

			try
			{
				// The Lazy<Task<T>> is unwrapped, and the actual async operation runs (if it wasn’t already).
				//If it was already running, we just await the same ongoing task.
				var data = await task!.Value;

				// This means the data isn’t valid to cache, so it’s removed to
				// avoid keeping a "null marker".
				if (data == null)
				{
					await RemoveAsync(key);
				}

				return data;
			}
			catch (Exception ex)
			{
				// The cache entry is removed (to prevent storing a failed state).
				await RemoveAsync(key);

				// NullReferenceException is treated specially — instead of rethrowing,
				// it returns default(T) (perhaps to handle unexpected nulls gracefully).
				if (ex is NullReferenceException)
				{
					return default;
				}

				throw ex;
			}
		}

		public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
		{
			var key = BuildKey(cacheKey, cacheKeyParameters).Key;
			memoryCache.Remove(key);
			cacheKeyManager.RemoveKey(key);

			return Task.CompletedTask;
		}
	}
}
