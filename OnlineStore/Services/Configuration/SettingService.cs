using GlideBuy.Core.Configuration;
using GlideBuy.Core.Domain.Configuration;

namespace GlideBuy.Services.Configuration
{
	public class SettingService : ISettingService
	{
		//public async Task<IList<Setting>> GetAllSettingsAsync()
		//{
		//	var settings
		//}

		//public async Task<ISettings> LoadSettingAsync(Type type)
		//{
		//	var settings = Activator.CreateInstance(type);

		//	// TODO: Check if the database is not installed.

		//	foreach (var prop in type.GetProperties())
		//	{
		//		if (!prop.CanRead || !prop.CanWrite)
		//		{
		//			continue;
		//		}


		//	}

		//	return settings as ISettings;
		//}
		public Task<ISettings> LoadSettingAsync(Type type)
		{
			throw new NotImplementedException();
		}
	}
}
