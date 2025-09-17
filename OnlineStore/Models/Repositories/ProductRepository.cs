using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Models.Repositories
{
	public class ProductRepository
	{
		private StoreDbContext context;

		public ProductRepository(StoreDbContext context)
		{
			this.context = context;
		}

		public IQueryable<Product> Products => context.Products;

		public void Add(Product entity)
		{
			context.Add(entity);
			context.SaveChanges();
		}

		public async Task AddAsync(Product entity)
		{
			await context.AddAsync(entity);
			await context.SaveChangesAsync();
		}

		public void Delete(Product entity)
		{
			context.Remove(entity);
			context.SaveChanges();
		}

		public async Task DeleteAsync(Product entity)
		{
			context.Remove(entity);
			await context.SaveChangesAsync();
		}

		// Update
		public void Save(Product entity)
		{
			context.SaveChanges();
		}

		public async Task SaveAsync(Product entity)
		{
			await context.SaveChangesAsync();
		}

		//void Add(IList<T> entitiies);
		//Task AddAsync(IList<T> entities);

		//void Save(IList<T> entities);
		//Task SaveAsync(IList<T> entities);

		//void Delete(IList<T> entities);
		//Task DeleteAsync(IList<T> entities);

		public Product? GetById(long? id, bool includeDeleted = true)
		{
			if (!id.HasValue || id == 0)
			{
				return null;
			}

			// TODO check for includeDeleted

			return context.Products
				.Include(p => p.Category)
				.FirstOrDefault(p => p.ProductId == id);
		}

		public async Task<Product?> GetByIdAsync(long? id, bool includeDeleted = true)
		{
			if (!id.HasValue || id == 0)
			{
				return null;
			}

			// TODO check for includeDeleted

			return await context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(p => p.ProductId == id);
		}

		//IList<T> GetByIds(IList<long> ids, bool includeDeleted = true);
		//Task<IList<T>> GetByIdsAsync(IList<long> ids, bool includeDeleted = true);

		//IList<T> GetAll(Func<IQueryable<T>, IQueryable<T>>? selector = null, bool includeDeleted = true);
		//Task<IList<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? selector = null, bool includeDeleted = true);
	}
}
