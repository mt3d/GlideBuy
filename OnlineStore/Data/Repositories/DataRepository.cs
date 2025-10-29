using Microsoft.EntityFrameworkCore;
using GlideBuy.Core.Caching;
using GlideBuy.Data;
using GlideBuy.Models.Common;
using GlideBuy.Models;

namespace GlideBuy.Data
{
	public class DataRepository<T> : IDataRepository<T> where T : BaseEntity
	{
		protected readonly StoreDbContext context;
		protected readonly IStaticCacheManager staticCacheManager;

		protected DbSet<T> Table;

		public DataRepository(StoreDbContext context, IStaticCacheManager staticCacheManager)
		{
			this.context = context;
			this.staticCacheManager = staticCacheManager;

			/**
			 * Entity Framework requires that this method return the same instance
			 * each time that it is called for a given context instance and entity type.
			 */
			Table = context.Set<T>();
		}

		/// <summary>
		/// Add an optional filter to a query, to determine if soft-deletable entitites
		/// should be returned or not.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="includeDeleted"></param>
		/// <returns></returns>
		private IQueryable<T> AddDeletedFilter(IQueryable<T> query, in bool includeDeleted)
		{
			// By default, EF Core queries will return "soft deleted" entities.
			// If the class do not implement ISoftDeletable, then do nothing.
			if (includeDeleted || typeof(T).GetInterface(nameof(ISoftDeletable)) == null)
			{
				return query;
			}

			return query.OfType<ISoftDeletable>().Where(row => row.Deleted != true).OfType<T>();
		}

		private async Task<IList<T>> ExecuteWithCachingAsync(
			Func<Task<IList<T>>> getData,
			Func<IStaticCacheManager, CacheKey> getCacheKey)
		{
			if (getCacheKey == null)
			{
				return await getData();
			}

			var cacheKey = getCacheKey(staticCacheManager)
				?? staticCacheManager.BuildKeyWithDefaultCacheTime(EntityCachingDefaults<T>.AllCacheKey);

			return await staticCacheManager.TryGetOrLoad(cacheKey, getData);
		}

		/// <summary>
		/// Get all entities.
		/// 
		/// The query modifier is a powerful pattern. It allows callers to dynamically
		/// modify the query before calling it. For instance:
		/// await productRepository.GetAllAsync(q => q.Where(p => p.Published).OrderBy(p => p.Name));
		/// 
		/// This exposes flexibility to the caller without forcing them to write raw EF queries
		/// </summary>
		/// <param name="queryModifier"></param>
		/// <param name="includeDeleted"></param>
		/// <returns></returns>
		public async Task<IList<T>> GetAllAsync(
			Func<IQueryable<T>, IQueryable<T>>? queryModifier = null,
			Func<ICacheKeyBuilder, CacheKey>? cachKeyFactory = null,
			bool includeDeleted = true)
		{
			// Separate querying logic from caching logic
			async Task<IList<T>> GetAllAsync()
			{
				var query = AddDeletedFilter(Table, includeDeleted);

				if (queryModifier != null)
				{
					queryModifier(query);
				}

				return await query.ToListAsync();
			}

			return await ExecuteWithCachingAsync(GetAllAsync, cachKeyFactory);
		}
	}
}
