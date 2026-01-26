using GlideBuy.Core.Caching;

namespace GlideBuy.Web.Infrastructure.Cache
{
	public static class ModelCacheDefaults
	{
		/// <summary>
		/// Key for caching of the categories displayed on the homepage.
		/// </summary>
		/// <remarks>
		/// TODO: Add the following placeholders.
		/// 
		/// {0} : store ID
		/// {1} : roles of the user
		/// {2} : picture size
		/// {3} : language ID
		/// {4} : is connection SSL secured (included in a category picture URL)
		/// </remarks>
		public static CacheKey CategoryHomepageKey => new("GlideBuy.ui.category.homepage");

		/// <summary>
		/// {0} : product ID
		/// {1} : picture size
		/// </summary>
		public static CacheKey ProductOverviewPicturesModelKey => new("GlideBuy.ui.product.overviewpictures-{0}-{1}");
	}
}
