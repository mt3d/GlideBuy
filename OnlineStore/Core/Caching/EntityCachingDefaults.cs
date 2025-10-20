using OnlineStore.Models;

namespace GlideBuy.Core.Caching
{
	public class EntityCachingDefaults<T> where T : BaseEntity
	{
		public static string EntityTypeName => typeof(T).Name.ToLowerInvariant();

		public static CacheKey AllCacheKey => new($"GlideBuy.{EntityTypeName}.all");
	}
}
