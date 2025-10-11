using OnlineStore.Models;

namespace OnlineStore.Services.ProductCatalog
{
	public interface IProductService
	{
		#region Products

		Product? GetProductById(long productId);
		Task<Product?> GetProductByIdAsync(long productId);

		// TODO: Use PagedList
		IList<Product> GetNewProducts(int pageIndex = 0, int pageSize = 0);

		Task<IList<Product>> GetProducts(long[] ids);

		Task InsertProductAsync(Product product);

		bool CheckProductAvailability(Product product, DateTime? dateTime = null);

		Task<string> FormatSkuAsync(Product product, string? attributesXml = null);

		#endregion
	}
}
