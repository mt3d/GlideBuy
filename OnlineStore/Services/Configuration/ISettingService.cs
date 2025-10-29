using GlideBuy.Core.Configuration;
using GlideBuy.Core.Domain.Configuration;

namespace GlideBuy.Services.Configuration
{
	public interface ISettingService
	{
		Task<IList<Setting>> GetAllSettingsAsync();

		Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default);

		Task<ISettings?> LoadSettingsAsync(Type type);
	}
}
