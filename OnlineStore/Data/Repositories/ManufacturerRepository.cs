using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineStore.Models;
using OnlineStore.Models.Common;
using System.Linq.Expressions;
using System.Transactions;

namespace OnlineStore.Data.Repositories
{
	public class ManufacturerRepository
	{
		private StoreDbContext context;

		public ManufacturerRepository(StoreDbContext context)
		{
			this.context = context;
		}

		public IQueryable<Manufacturer> Manufacturers => context.Manufacturers;

		private IQueryable<Manufacturer> AddDeletedFilter(IQueryable<Manufacturer> query, bool includeDeleted)
		{
			// By default, EF Core queries will return "soft deleted" entities.
			if (includeDeleted)
			{
				return query;
			}

			// If the class do not implement ISoftDeletable, then do nothing.
			if (typeof(Manufacturer).GetInterface(nameof(ISoftDeletable)) == null)
			{
				return query;
			}

			// Generic version: query.OfType<ISoftDeletable>, since we can't be sure
			// that the entity has a Deleted property.
			return query.Where(entity => !entity.Deleted);
		}

		// TODO: Implement caching.
		public async Task<Manufacturer?> GetByIdAsync(int id, bool includeDeleted = true)
		{
			return await AddDeletedFilter(Manufacturers, includeDeleted).FirstOrDefaultAsync(manufacturer => manufacturer.ManufacturerId == id);

			// TODO: Implement conditional caching.
		}

		// TODO: Implement caching.
		public Manufacturer? GetById(int id, bool includeDeleted = true)
		{
			return AddDeletedFilter(Manufacturers, includeDeleted).FirstOrDefault(manufacturer => manufacturer.ManufacturerId == id);

			// TODO: Implement conditional caching.
		}

		//public async Task<IList<Manufacturer>> GetByIdsAsync(IList<int> ids, bool includeDeleted = true)
		//{
		//	// The sequence can be null, and it can contains no elements. Return an empty
		//	// list in these cases.
		//	if (ids?.Any() != true)
		//		return new List<Manufacturer>();

		//	static IList<TEntity> sortByIdList(IList<int> listOfId, IDictionary<int, TEntity> entitiesById)
		//	{
		//		var sortedEntities = new List<TEntity>(listOfId.Count);

		//		foreach (var id in listOfId)
		//			if (entitiesById.TryGetValue(id, out var entry))
		//				sortedEntities.Add(entry);

		//		return sortedEntities;
		//	}

		//	async Task<IList<TEntity>> getByIdsAsync(IList<int> listOfId, bool sort = true)
		//	{
		//		var query = AddDeletedFilter(Table, includeDeleted)
		//			.Where(entry => listOfId.Contains(entry.Id));

		//		return sort
		//			? sortByIdList(listOfId, await query.ToDictionaryAsync(entry => entry.Id))
		//			: await query.ToListAsync();
		//	}

		//	if (getCacheKey == null)
		//		return await getByIdsAsync(ids);
		//}

		//public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
		//	Func<ICacheKeyService, CacheKey> getCacheKey = null, bool includeDeleted = true)
		//{
		//	async Task<IList<TEntity>> getAllAsync()
		//	{
		//		var query = AddDeletedFilter(Table, includeDeleted);
		//		query = func != null ? func(query) : query;

		//		return await query.ToListAsync();
		//	}

		//	return await GetEntitiesAsync(getAllAsync, getCacheKey);
		//}

		//public virtual IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
		//	Func<ICacheKeyService, CacheKey> getCacheKey = null, bool includeDeleted = true)
		//{
		//	IList<TEntity> getAll()
		//	{
		//		var query = AddDeletedFilter(Table, includeDeleted);
		//		query = func != null ? func(query) : query;

		//		return query.ToList();
		//	}

		//	return GetEntities(getAll, getCacheKey);
		//}

		//public virtual async Task<IList<TEntity>> GetAllAsync(
		//	Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
		//	Func<ICacheKeyService, CacheKey> getCacheKey = null, bool includeDeleted = true)
		//{
		//	async Task<IList<TEntity>> getAllAsync()
		//	{
		//		var query = AddDeletedFilter(Table, includeDeleted);
		//		query = func != null ? await func(query) : query;

		//		return await query.ToListAsync();
		//	}

		//	return await GetEntitiesAsync(getAllAsync, getCacheKey);
		//}

		//public virtual async Task<IList<TEntity>> GetAllAsync(
		//	Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
		//	Func<ICacheKeyService, Task<CacheKey>> getCacheKey = null, bool includeDeleted = true)
		//{
		//	async Task<IList<TEntity>> getAllAsync()
		//	{
		//		var query = AddDeletedFilter(Table, includeDeleted);
		//		query = func != null ? await func(query) : query;

		//		return await query.ToListAsync();
		//	}

		//	return await GetEntitiesAsync(getAllAsync, getCacheKey);
		//}

		//public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
		//	int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true)
		//{
		//	var query = AddDeletedFilter(Table, includeDeleted);

		//	query = func != null ? func(query) : query;

		//	return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
		//}

		//public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
		//	int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true)
		//{
		//	var query = AddDeletedFilter(Table, includeDeleted);

		//	query = func != null ? await func(query) : query;

		//	return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
		//}

		//public virtual async Task InsertAsync(TEntity entity, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entity);

		//	await _dataProvider.InsertEntityAsync(entity);

		//	//event notification
		//	if (publishEvent)
		//		await _eventPublisher.EntityInsertedAsync(entity);
		//}

		//public virtual void Insert(TEntity entity, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entity);

		//	_dataProvider.InsertEntity(entity);

		//	//event notification
		//	if (publishEvent)
		//		_eventPublisher.EntityInserted(entity);
		//}

		//public virtual async Task InsertAsync(IList<TEntity> entities, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entities);

		//	using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
		//	await _dataProvider.BulkInsertEntitiesAsync(entities);
		//	transaction.Complete();

		//	if (!publishEvent)
		//		return;

		//	//event notification
		//	foreach (var entity in entities)
		//		await _eventPublisher.EntityInsertedAsync(entity);
		//}

		//public virtual void Insert(IList<TEntity> entities, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entities);

		//	using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
		//	_dataProvider.BulkInsertEntities(entities);
		//	transaction.Complete();

		//	if (!publishEvent)
		//		return;

		//	//event notification
		//	foreach (var entity in entities)
		//		_eventPublisher.EntityInserted(entity);
		//}

		//public virtual async Task<TEntity> LoadOriginalCopyAsync(TEntity entity)
		//{
		//	return await _dataProvider.GetTable<TEntity>()
		//		.FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(entity.Id));
		//}

		//public virtual async Task UpdateAsync(TEntity entity, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entity);

		//	await _dataProvider.UpdateEntityAsync(entity);

		//	//event notification
		//	if (publishEvent)
		//		await _eventPublisher.EntityUpdatedAsync(entity);
		//}

		//public virtual void Update(TEntity entity, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entity);

		//	_dataProvider.UpdateEntity(entity);

		//	//event notification
		//	if (publishEvent)
		//		_eventPublisher.EntityUpdated(entity);
		//}

		//public virtual async Task UpdateAsync(IList<TEntity> entities, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entities);

		//	if (!entities.Any())
		//		return;

		//	await _dataProvider.UpdateEntitiesAsync(entities);

		//	//event notification
		//	if (!publishEvent)
		//		return;

		//	foreach (var entity in entities)
		//		await _eventPublisher.EntityUpdatedAsync(entity);
		//}

		//public virtual void Update(IList<TEntity> entities, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entities);

		//	if (!entities.Any())
		//		return;

		//	_dataProvider.UpdateEntities(entities);

		//	//event notification
		//	if (!publishEvent)
		//		return;

		//	foreach (var entity in entities)
		//		_eventPublisher.EntityUpdated(entity);
		//}

		//public virtual async Task DeleteAsync(TEntity entity, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entity);

		//	switch (entity)
		//	{
		//		case ISoftDeletedEntity softDeletedEntity:
		//			softDeletedEntity.Deleted = true;
		//			await _dataProvider.UpdateEntityAsync(entity);
		//			break;

		//		default:
		//			await _dataProvider.DeleteEntityAsync(entity);
		//			break;
		//	}

		//	//event notification
		//	if (publishEvent)
		//		await _eventPublisher.EntityDeletedAsync(entity);
		//}

		///// <summary>
		///// Delete the entity entry
		///// </summary>
		///// <param name="entity">Entity entry</param>
		///// <param name="publishEvent">Whether to publish event notification</param>
		//public virtual void Delete(TEntity entity, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entity);

		//	switch (entity)
		//	{
		//		case ISoftDeletedEntity softDeletedEntity:
		//			softDeletedEntity.Deleted = true;
		//			_dataProvider.UpdateEntity(entity);
		//			break;

		//		default:
		//			_dataProvider.DeleteEntity(entity);
		//			break;
		//	}

		//	//event notification
		//	if (publishEvent)
		//		_eventPublisher.EntityDeleted(entity);
		//}

		//public virtual async Task DeleteAsync(IList<TEntity> entities, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entities);

		//	if (!entities.Any())
		//		return;

		//	using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		//	if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) == null)
		//		await _dataProvider.BulkDeleteEntitiesAsync(entities);
		//	else
		//	{
		//		foreach (var entity in entities)
		//			((ISoftDeletedEntity)entity).Deleted = true;

		//		await _dataProvider.UpdateEntitiesAsync(entities);
		//	}

		//	transaction.Complete();

		//	//event notification
		//	if (!publishEvent)
		//		return;

		//	foreach (var entity in entities)
		//		await _eventPublisher.EntityDeletedAsync(entity);
		//}

		//public virtual void Delete(IList<TEntity> entities, bool publishEvent = true)
		//{
		//	ArgumentNullException.ThrowIfNull(entities);

		//	if (!entities.Any())
		//		return;

		//	using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		//	if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) == null)
		//		_dataProvider.BulkDeleteEntities(entities);
		//	else
		//	{
		//		foreach (var entity in entities)
		//			((ISoftDeletedEntity)entity).Deleted = true;

		//		_dataProvider.UpdateEntities(entities);
		//	}

		//	transaction.Complete();

		//	//event notification
		//	if (!publishEvent)
		//		return;

		//	foreach (var entity in entities)
		//		_eventPublisher.EntityDeleted(entity);
		//}

		//public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
		//{
		//	ArgumentNullException.ThrowIfNull(predicate);

		//	using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
		//	var countDeletedRecords = await _dataProvider.BulkDeleteEntitiesAsync(predicate);
		//	transaction.Complete();

		//	return countDeletedRecords;
		//}

		//public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
		//{
		//	ArgumentNullException.ThrowIfNull(predicate);

		//	using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
		//	var countDeletedRecords = _dataProvider.BulkDeleteEntities(predicate);
		//	transaction.Complete();

		//	return countDeletedRecords;
		//}
	}
}
