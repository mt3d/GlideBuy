using GlideBuy.Data.Repositories;
using GlideBuy.Models;
using GlideBuy.Core.Domain.Catalog;

namespace GlideBuy.Services.ProductCatalog
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

		public async Task<string> FormatSkuAsync(Product product, string? attributesXml = null)
		{
			ArgumentNullException.ThrowIfNull(product);

			var (sku, _, _) = await GetSkuMpnGtinAsync(product, attributesXml);

			return sku;
		}

		/// <summary>
		/// Retrieve the correct product identifiers (SKU, Manufacturer Part Number, and GTIN)
		/// for a given product, taking into account its selected attributes (like size, color, etc.),
		/// not just the base product.
		/// </summary>
		/// <param name="product"></param>
		/// <param name="attributes"></param>
		/// <param name=""></param>
		/// <returns></returns>
		private async Task<(string sku, string mpn, string gtin)> GetSkuMpnGtinAsync(Product product, string attributesXml)
		{
			ArgumentNullException.ThrowIfNull(product);

			string? sku = null;
			string? mpn = null;
			string? gtin = null;

			// Handle the case when the prodcut have a stock for each variation.
			if (product.InventoryManagementMethod == InventoryManagementMethod.ManageStockByAttributes
				&& !string.IsNullOrEmpty(attributesXml))
			{
				throw new NotImplementedException();
				// TODO: Implement finding identifiers by attribute combinations.
			}

			// If the product doesn't manage stock by attributes, fallback to the base product identifiers.
			if (string.IsNullOrEmpty(sku))
			{
				sku = product.Sku;
			}
			if (string.IsNullOrEmpty(mpn))
			{
				mpn = product.ManufacturerPartNumber;
			}
			if (string.IsNullOrEmpty(gtin))
			{
				gtin = product.Gtin;
			}

			return (sku, mpn, gtin);
		}
	}
}
