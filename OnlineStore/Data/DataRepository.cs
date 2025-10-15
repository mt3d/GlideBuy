using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models.Common;

namespace GlideBuy.Data
{
	public class DataRepository<T> : IRepository<T> where T : class
	{
		protected readonly StoreDbContext context;
		protected DbSet<T> Table;

		public DataRepository(StoreDbContext context)
		{
			this.context = context;

			/**
			 * Entity Framework requires that this method return the same instance
			 * each time that it is called for a given context instance and entity type.
			 */
			Table = context.Set<T>();
		}

		/// <summary>
		/// Determines if soft deleted entities should be returned,
		/// When the query applies to <see cref="ISoftDeletable"/> entities.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="includeDeleted"></param>
		/// <returns></returns>
		private IQueryable<T> ReturnDeleted(IQueryable<T> query, in bool includeDeleted)
		{
			// By default, EF Core queries will return "soft deleted" entities.
			// If the class do not implement ISoftDeletable, then do nothing.
			if (includeDeleted || typeof(T).GetInterface(nameof(ISoftDeletable)) == null)
			{
				return query;
			}

			return query.OfType<ISoftDeletable>().Where(row => row.Deleted != true).OfType<T>();
		}
	}
}
