using GlideBuy.Core.Caching;

namespace GlideBuy.Services.Configuration
{
	public static class SettingsCashingDefaults
	{
		public static CacheKey AllSettingsAsDictionaryCacheKey => new("GlideBuy.settings.all.dictionary");
	}
}
