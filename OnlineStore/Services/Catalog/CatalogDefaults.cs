using GlideBuy.Core.Caching;

namespace GlideBuy.Services.Catalog
{
	public class CatalogDefaults
	{
		public static CacheKey ProductsHomepageCacheKey => new("GlideBuy.product.homepage.");
	}
}
