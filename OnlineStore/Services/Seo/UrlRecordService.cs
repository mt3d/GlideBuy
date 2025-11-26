using GlideBuy.Core.Caching;
using GlideBuy.Core.Domain.Seo;
using GlideBuy.Data;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Services.Seo
{
	public class UrlRecordService : IUrlRecordService
	{
		private readonly IStaticCacheManager _staticCacheManager;
		private readonly IDataRepository<UrlRecord> _urlRecordRepository;
		
		public UrlRecordService(
			IStaticCacheManager staticCacheManager,
			IDataRepository<UrlRecord> urlRecordRepository)
		{
			_staticCacheManager = staticCacheManager;
			_urlRecordRepository = urlRecordRepository;
		}

		public async Task<UrlRecord?> GetBySlugAsync(string slug)
		{
			if (string.IsNullOrEmpty(slug))
			{
				return null;
			}

			// TODO: Check if all URL records are loaded on startup

			// Gradual loading.
			CacheKey key = _staticCacheManager.BuildKeyWithDefaultCacheTime(SeoDefaults.UrlRecordBySlugCacheKey, slug);

			return await _staticCacheManager.TryGetOrLoad(key, async () =>
			{
				var query = _urlRecordRepository.Table
					.Where(ur => ur.Slug == slug)
					.OrderByDescending(ur => ur.IsActive)
					.OrderBy(ur => ur.Id);

				return await query.FirstOrDefaultAsync();
			});
		}
	}
}
