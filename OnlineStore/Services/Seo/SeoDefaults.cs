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
	}
}
