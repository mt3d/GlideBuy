using GlideBuy.Core.Domain.Seo;

namespace GlideBuy.Services.Seo
{
	public interface IUrlRecordService
	{
		Task<UrlRecord?> GetBySlugAsync(string slug);
	}
}
