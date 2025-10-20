using GlideBuy.Core.Configuration;

namespace GlideBuy.Services.Configuration
{
	public interface ISettingService
	{
		Task<ISettings> LoadSettingAsync(Type type);
	}
}
