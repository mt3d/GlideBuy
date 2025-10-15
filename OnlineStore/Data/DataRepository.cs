using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;

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
	}
}
