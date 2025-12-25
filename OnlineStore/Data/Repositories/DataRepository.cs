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
		protected readonly IStaticCacheManager _staticCacheManager;

		public IQueryable<T> Table { get; }

		public DataRepository(StoreDbContext context, IStaticCacheManager staticCacheManager)
		{
			this.context = context;
			this._staticCacheManager = staticCacheManager;

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
			Func<IStaticCacheManager, CacheKey>? getCacheKey)
		{
			if (getCacheKey == null)
			{
				return await getData();
			}

			var cacheKey = getCacheKey(_staticCacheManager)
				?? _staticCacheManager.BuildKeyWithDefaultCacheTime(EntityCachingDefaults<T>.AllCacheKey);

			return await _staticCacheManager.TryGetOrLoad(cacheKey, getData);
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

		public async Task InsertAsync(T entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			await context.AddAsync<T>(entity);
			await context.SaveChangesAsync();

			// TODO: Publish event
		}

		public async Task UpdateAsync(T entity, bool publishEvent = true)
		{
			ArgumentNullException.ThrowIfNull(entity);

			context.Update(entity); // no database I/O performed
			await context.SaveChangesAsync();

			if (publishEvent)
			{
				// TODO: Publish an event.
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// If getCacheKey is null, then caching will not be used. However, if a function
		/// that returns a default is passed, then the default cache key will be used.
		/// </remarks>
		public async Task<T?> GetByIdAsync(int? id, Func<ICacheKeyBuilder, CacheKey> getCacheKey = null, bool includeDeleted = true)
		{
			if (!id.HasValue || id == 0)
			{
				return null;
			}

			async Task<T?> getEntityAsync()
			{
				return await AddDeletedFilter(Table, includeDeleted).FirstOrDefaultAsync(entity => entity.Id == id);
			}

			if (getCacheKey == null)
			{
				return await getEntityAsync();
			}

			// TODO: Add an option to use a short-term cach manager
			ICacheKeyBuilder cacheKeyBuilder = _staticCacheManager;

			var cacheKey = getCacheKey(cacheKeyBuilder)
							?? cacheKeyBuilder.BuildKeyWithDefaultCacheTime(EntityCachingDefaults<T>.ByIdCacheKey, id);

			return await _staticCacheManager.TryGetOrLoad(cacheKey, getEntityAsync);
		}
	}
}
