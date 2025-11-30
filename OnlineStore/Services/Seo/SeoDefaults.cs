using GlideBuy.Core.Caching;

namespace GlideBuy.Services.Seo
{
	public static class SeoDefaults
	{
		/// <summary>
		/// Gets a key for caching slugs.
		/// </summary>
		/// <remarks>
		/// {0} : slug
		/// </remarks>
		public static CacheKey UrlRecordBySlugCacheKey => new("GlideBuy.urlrecord.byslug.{0}");

		/// <summary>
		/// Gets a key for caching
		/// </summary>
		/// <remarks>
		/// {0} : entity ID
		/// {1} : entity name
		/// {2} : language ID
		/// </remarks>
		public static CacheKey UrlRecordCacheKey => new("GlideBuy.urlrecord.{0}-{1}-{2}");
	}
}
