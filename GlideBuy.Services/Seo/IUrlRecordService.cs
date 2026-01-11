using GlideBuy.Core.Domain.Seo;
using GlideBuy.Models;

namespace GlideBuy.Services.Seo
{
	public interface IUrlRecordService
	{
		Task<UrlRecord?> GetBySlugAsync(string slug);

		Task<string> GetSeNameAsync<T>(T entity, int? languageId = null, bool returnDefaultValue = true) where T : BaseEntity, ISlugSupported;
	}
}
