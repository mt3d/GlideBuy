using GlideBuy.Core.Caching;

namespace GlideBuy.Data
{
	public interface IDataRepository<T>
	{
		//	IQueryable<T> All { get; }

		//	void Add(IList<T> entitiies);
		//	Task AddAsync(IList<T> entities);

		//	// Aka Update
		//	void Save(T entity);
		//	Task SaveAsync(T entity);

		//	void Save(IList<T> entities);
		//	Task SaveAsync(IList<T> entities);

		//	void Delete(T entity);
		//	Task DeleteAsync(T entity);

		//	void Delete(IList<T> entities);
		//	Task DeleteAsync(IList<T> entities);

		//	T? GetById(long? id, bool includeDeleted = true);
		//	Task<T?> GetByIdAsync(long? id, bool includeDeleted = true);

		//	IList<T> GetByIds(IList<long> ids, bool includeDeleted = true);
		//	Task<IList<T>> GetByIdsAsync(IList<long> ids, bool includeDeleted = true);

		Task<IList<T>> GetAllAsync(
			Func<IQueryable<T>, IQueryable<T>>? queryModifier = null,
			Func<ICacheKeyBuilder, CacheKey>? cachKeyFactory = null,
			bool includeDeleted = true);

		Task InsertAsync(T entity);

		Task UpdateAsync(T entity, bool publishEvent = true);
	}
}
