namespace GlideBuy.Core.Caching
{
	/// <summary>
	/// A manager for caching items between HTTP requests. Used to store data
	/// that does not change very often.
	/// 
	/// The implementation could be distributed or in-memory.
	/// </summary>
	public interface IStaticCacheManager : IDisposable, ICacheKeyBuilder
	{
		// TODO: Rename to TryGetOrLoadAsync
		/// <summary>
		/// Get a cached item. If the item is not found, then load it and cache it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="acquire">A function to asynchronously load the item if it's not in the cache.</param>
		/// <returns></returns>
		Task<T> TryGetOrLoad<T>(CacheKey key, Func<Task<T>> acquire);

		Task<T> TryGetOrLoadAsync<T>(CacheKey key, Func<T> acquire);

		/// <summary>
		/// Remove the value with the specified key from the cache store.
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <param name="cacheKeyParameters"></param>
		/// <returns></returns>
		Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters);

		Task SetAsync<T>(CacheKey cacheKey, T data);
	}
}
