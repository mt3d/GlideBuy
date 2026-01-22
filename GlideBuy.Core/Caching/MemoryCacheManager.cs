
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace GlideBuy.Core.Caching
{
	public class MemoryCacheManager : CacheKeyBuilder, IStaticCacheManager
	{
		protected readonly IMemoryCache memoryCache;
		protected readonly ICacheKeyManager cacheKeyManager;

		protected CancellationTokenSource _clearToken = new();

		public MemoryCacheManager(IMemoryCache memoryCache, ICacheKeyManager cacheKeyManager)
		{
			this.memoryCache = memoryCache;
			this.cacheKeyManager = cacheKeyManager;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// TODO: Ensure that Dispose() is called only once.

			if (disposing)
			{
				// Call Dispose() on other objects owned by this instance.
				// We can reference othe finalizable objects here.

				// TODO: Use in the future to dispose a CancellationTokenSource
			}
		}

		public async Task<T> TryGetOrLoadAsync<T>(CacheKey? key, Func<T> acquire)
		{
			return await TryGetOrLoad(key, () => Task.FromResult(acquire()));
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
		public async Task<T> TryGetOrLoad<T>(CacheKey? key, Func<Task<T>> acquire)
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

				throw;
			}
		}

		public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
		{
			var key = BuildKey(cacheKey, cacheKeyParameters).Key;
			memoryCache.Remove(key);
			cacheKeyManager.RemoveKey(key);

			return Task.CompletedTask;
		}

		public Task SetAsync<T>(CacheKey? cacheKey, T data)
		{
			/**
			 * The conditional guard at the beginning ensures that null values and zero or negative cache durations are not cached. This avoids polluting the cache with meaningless entries and prevents subtle bugs where a null value becomes indistinguishable from a cache miss. If caching is disabled for a given key by setting CacheTime to zero, the method becomes a no-op.
			 */
			if (data != null && (cacheKey?.CacheTimeMinute ?? 0) > 0)
			{
				/**
				 * Lazy is like a factory of the value, not the value itself.
				 * 
				 * The boolean true passed to the Lazy constructor enforces thread safety, guaranteeing that the value factory cannot be executed concurrently.
				 * 
				 * In short, Lazy is not protecting the act of storing the value in the cache; it is protecting the execution of the value-producing function associated with that cache entry. Even when that function is trivial, the pattern remains the same to keep the caching infrastructure consistent and safe under concurrent access.
				 */
				memoryCache.Set(
					cacheKey.Key,
					new Lazy<Task<T>>(() => Task.FromResult(data), true),
					PrepareEntryOptions(cacheKey)
					);
			}

			/**
			 * The SetAsync method then uses these prepared options to actually insert data into the cache. Although the method is asynchronous by signature, it does not perform any truly asynchronous work. This is intentional and reflects an API design choice rather than an implementation constraint.
			 * 
			 * The system exposes an async caching API to allow it to be consumed uniformly from async code paths without forcing callers to branch between sync and async variants. Returning Task.CompletedTask is simply a way to satisfy the async contract without introducing unnecessary overhead.
			 */
			return Task.CompletedTask;
		}

		/**
		 * The method PrepareEntryOptions is responsible for translating a logical
		 * CacheKey into concrete cache eviction rules understood by Microsoft.Extensions.Caching.Memory.
		 */
		private MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
		{
			var options = new MemoryCacheEntryOptions
			{
				/**
				 * The method ensures that the cached value is guaranteed to expire
				 * after a fixed duration regardless of access frequency. This choice
				 * deliberately avoids sliding expiration because many cache entries 
				 * represent data that must be refreshed periodically even if it is
				 * frequently read, such as settings, permissions, or configuration-derived data.
				 */
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(key.CacheTimeMinute)
			};

			/**
			 * Adding a CancellationChangeToken based on _clearToken.Token, is the
			 * mechanism that allows the system to invalidate large portions of the
			 * cache proactively rather than waiting for time-based expiration.
			 * 
			 * _clearToken is typically backed by a CancellationTokenSource that is reset whenever the system decides that cached data is no longer trustworthy, for example after an entity update, plugin installation, or configuration change. When the token is canceled, every cache entry that registered this token becomes expired immediately.
			 * 
			 * This is a crucial point: instead of tracking and removing individual cache keys one by one, the system can invalidate an entire logical cache region in a single operation by canceling the token.
			 */
			options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
			options.RegisterPostEvictionCallback(OnEviction);

			return options;
		}

		/**
		 * The cache itself can evict entries for several different reasons, and not all of those reasons imply that Nop should forget about the key entirely. This method exists to decide when it is safe and appropriate to remove a cache key from the internal key manager.
		 */
		private void OnEviction(object key, object? value, EvictionReason reason, object? state)
		{
			switch (reason)
			{
				/**
				 * When the reason is Removed, it means that the application itself deliberately removed the cache entry, typically as part of a targeted cache invalidation routine. In this case, Nop already knows that the key is no longer valid and will handle cleanup elsewhere, so removing it again here would be redundant or even harmful. When the reason is Replaced, it means the cache entry was overwritten with a new value under the same key. In that scenario, the key is still valid and actively in use, so removing it from the key manager would desynchronize internal state. TokenExpired indicates that a cancellation token triggered expiration, which in Nop usually corresponds to a deliberate, bulk invalidation event. Again, the system already has higher-level logic that accounts for this, so the eviction callback does nothing.
				 */
				case EvictionReason.Removed:
				case EvictionReason.Replaced:
				case EvictionReason.TokenExpired:
					break;
				/**
				 * The default case covers all other eviction reasons, most notably eviction due to memory pressure. This is the critical path for this method. When the memory cache evicts an entry because it needs to free space, that eviction is initiated internally by the cache, not by Nop’s cache management logic. In that situation, the key manager would still believe the key exists unless explicitly told otherwise. This would leave behind stale metadata about a cache entry that no longer exists, which could cause incorrect behavior in diagnostics, cache clearing operations, or administrative tooling. Therefore, in these cases, the key must be removed from the key manager to keep internal state consistent with the actual contents of the cache.
				 */
				default:
					/**
					 * The conditional check using _memoryCache.TryGetValue(key, out _) addresses a subtle race condition. Eviction callbacks are not guaranteed to run in isolation from other cache operations. It is possible for an entry to be evicted and then immediately re-added under the same key before the eviction callback executes. If the callback blindly removed the key from the key manager, it would accidentally remove a key that now corresponds to a valid, newly added cache entry. By checking whether the key currently exists in the cache at the time of the callback, the code ensures that it only removes the key if it truly no longer exists. This prevents accidental corruption of the key registry.
					 */
					if (!memoryCache.TryGetValue(key, out _))
					{
						cacheKeyManager.RemoveKey(key as string);
					}
					break;
			}
		}
	}
}
