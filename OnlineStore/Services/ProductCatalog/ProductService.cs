using OnlineStore.Models;
using OnlineStore.Models.Repositories;

namespace OnlineStore.Services.ProductCatalog
{
	public class ProductService : IProductService
	{
		protected readonly ProductRepository productRepository;

		public ProductService(ProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		public Product? GetProductById(long productId)
		{
			return productRepository.GetById(productId);
		}

		public async Task<Product?> GetProductByIdAsync(long productId)
		{
			return await productRepository.GetByIdAsync(productId);
		}

		public bool CheckProductAvailability(Product product, DateTime? dateTime = null)
		{
			throw new NotImplementedException();
		}

		public IList<Product> GetNewProducts(int pageIndex = 0, int pageSize = 0)
		{
			// TODO: Check that the product is published.
			// TODO: Check if the prodcut is visible individually or just in group
			// TODO: One could specify a time period (start date => end date), that specify
			//		 when products are marked as new.
			var query = from p in productRepository.Products
						where p.MarkedAsNew && !p.Deleted
						select p;

			// TODO: In the future, in case you want to support multiple stores,
			// you need to add a modification to the query, so that only the products
			// from the specified store are retrieved.

			// TODO: Apply ACL constraints

			// TODO: Order by creation time.

			// TODO: Consider using paged Lists

			return query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}

		public async Task<Product?> GetProductByIdAsync(int productId)
		{
			return await productRepository.GetByIdAsync(productId);
		}

		public Task<IList<Product>> GetProducts(long[] ids)
		{
			throw new NotImplementedException();
		}

		public Task InsertProductAsync(Product product)
		{
			throw new NotImplementedException();
		}
	}
}
