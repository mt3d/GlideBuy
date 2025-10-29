using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GlideBuy.Models;
using GlideBuy.Models.Common;
using System.Linq.Expressions;
using System.Transactions;

namespace GlideBuy.Data.Repositories
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
	}
}
