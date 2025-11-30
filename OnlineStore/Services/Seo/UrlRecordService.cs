using GlideBuy.Core.Caching;
using GlideBuy.Core.Domain.Seo;
using GlideBuy.Data;
using GlideBuy.Models;
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

		// Used in SlugRouteTransformer
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

		// Used when preparing the CategoryModel for instance.
		public async Task<string> GetSeNameAsync<T>(T entity, int? languageId = null, bool returnDefaultValue = true) where T : BaseEntity, ISlugSupported
		{
			ArgumentNullException.ThrowIfNull(entity);

			var entityName = entity.GetType().Name;

			return await GetSeNameAsync(entity.Id, entityName, languageId, returnDefaultValue);
		}

		public async Task<string> GetSeNameAsync(int entityId, string entityName, int? languageId = null, bool returnDefaultValue = true)
		{
			// TODO: Get the language.

			var result = string.Empty;

			// 1. Try to get a localized search engine name
			// TODO: Load localized version by suppylign the language Id to GetActiveSlugAsync

			// 2. If not found, try to get the default value if required.
			if (string.IsNullOrEmpty(result) && returnDefaultValue)
			{
				result = await GetActiveSlugAsync(entityId, entityName, 0);
			}

			return result;
		}

		public async Task<string?> GetActiveSlugAsync(int entityId, string entityTypeName, int languageId)
		{
			// TODO: Check if all URL records were loaded on startup.

			// Gradual loading.
			CacheKey key = _staticCacheManager.BuildKeyWithDefaultCacheTime(SeoDefaults.UrlRecordCacheKey, entityId, entityTypeName, languageId);

			return await _staticCacheManager.TryGetOrLoad(key, async () =>
			{
				var query = _urlRecordRepository.Table
					.Where(ur => ur.EntityId == entityId && ur.EntityName == entityTypeName && ur.LanguageId == languageId && ur.IsActive)
					.OrderByDescending(ur => ur.Id)
					.Select(ur => ur.Slug);

				return await query.FirstOrDefaultAsync();
			});
		}
	}
}
