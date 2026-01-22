using GlideBuy.Core;
using GlideBuy.Models;

namespace GlideBuy.Services.Catalog
{
	public interface IProductService
	{
		#region Products

		Product? GetProductById(int productId);

		Task<Product?> GetProductByIdAsync(int productId);

		// TODO: Use PagedList
		IList<Product> GetNewProducts(int pageIndex = 0, int pageSize = 0);

		Task<IList<Product>> GetProducts(long[] ids);

		Task InsertProductAsync(Product product);

		bool CheckProductAvailability(Product product, DateTime? dateTime = null);

		Task<string> FormatSkuAsync(Product product, string? attributesXml = null);

		Task<IList<Product>> GetAllProductsDisplayedOnHomepageAsync();

		Task<IPagedList<Product>> SearchProductAsync(
			int pageIndex = 0,
			int pageSize = int.MaxValue,
			IList<int>? categoryIds = null);

		Task<IList<Product>> GetNewlyArrivedProducts(int count);

		Task<IList<Product>> GetHomepageTrendingProductsAsync(int count);

		#endregion
	}
}
