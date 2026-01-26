using GlideBuy.Core.Caching;
using GlideBuy.Core.Configuration;
using GlideBuy.Core.Domain.Configuration;
using GlideBuy.Data;
using GlideBuy.Core;
using System.ComponentModel;

namespace GlideBuy.Services.Configuration
{
	public class SettingService : ISettingService
	{
		private readonly IDataRepository<Setting> settingRepository;
		private readonly IStaticCacheManager staticCacheManager;

		public SettingService(IStaticCacheManager staticCacheManager, IDataRepository<Setting> settingRepository)
		{
			this.staticCacheManager = staticCacheManager;
			this.settingRepository = settingRepository;
		}

		/// <summary>
		/// Gets all the settings stored in the database.
		/// </summary>
		/// <returns></returns>
		public async Task<IList<Setting>> GetAllSettingsAsync()
		{
			var settings = await settingRepository.GetAllAsync(query =>
			{
				return query.OrderBy(s => s.Name);
			}, cacheKeyFacory => default);

			return settings;
		}

		/// <summary>
		/// For multi-store applications. Not needed currently.
		/// 
		/// Builds and cache a dictionary of settings. The key is the setting's name lowercased.
		/// The value is a list of Setting objects — one for each store-specific override
		/// (so there can be multiple entries with the same name but different StoreIds).
		/// </summary>
		/// <returns></returns>
		private async Task<IDictionary<string, IList<Setting>>> GetAllSettingsDictionaryAsync()
		{
			return await staticCacheManager.TryGetOrLoadAsync(SettingsCashingDefaults.AllSettingsAsDictionaryCacheKey, async () =>
			{
				var settings = await GetAllSettingsAsync();

				var dictionary = new Dictionary<string, IList<Setting>>();

				foreach (Setting s in settings)
				{
					// Example name: "OrderSettings.MinOrderTotalAmount"
					var name = s.Name.ToLowerInvariant();

					// Do not store tracked entities in the cache.
					var settingCopy = new Setting
					{
						Id = s.Id,
						Name = s.Name,
						Value = s.Value
					};

					if (dictionary.TryGetValue(name, out var value))
					{
						value.Add(settingCopy);
					}
					else
					{
						dictionary.Add(name, new List<Setting>
						{
							settingCopy
						});
					}
				}

				return dictionary;
			});
		}

		public async Task<string?> GetRawSettingByKeyAsync(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}

			var settingsDictionary = await GetAllSettingsDictionaryAsync();

			key = key.Trim().ToLowerInvariant();
			if (!settingsDictionary.TryGetValue(key, out var settings))
			{
				return null;
			}

			/**
			 * TODO: Since we're not supporting multiple stores currently,
			 * each list will have just a single element. Later on,
			 * we'll need to compare store IDs.
			 */
			Setting? setting = settings.First();

			// TODO: Later on, we'll need to support shared values for all stores
			// in case we didn't find a value for the specified one.

			return setting?.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">Key format: {TypeName}.{PropertyName}
		/// For instance: OrderSettings.MinOrderTotalAmount</typeparam>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public async Task<T?> GetSettingByKeyAsync<T>(string key, T? defaultValue = default)
		{
			var value = await GetRawSettingByKeyAsync(key);

			return value != null
				? ConversionHelper.ConvertTo<T>(value)
				: defaultValue;
		}

		public async Task<ISettings?> LoadSettingsAsync(Type type)
		{
			var settings = Activator.CreateInstance(type);

			// TODO: Check if the database is not installed
			// TODO: return if true

			foreach (var prop in type.GetProperties())
			{
				// TODO: Give examples.
				if (!prop.CanRead || !prop.CanWrite)
				{
					continue;
				}

				var key = type.Name + "." + prop.Name;

				// In this instance, we cannot benefit from the conversion provided by
				// GetSettingByKeyAsync(), because there's no way to know the property type
				// at compile time. You can't write something like
				// GetSettingByKeyAsync<prop.PropertyType>(key).
				// PropertyType could be anything.
				string? stringValue = await GetRawSettingByKeyAsync(key);
				if (stringValue == null)
				{
					continue;
				}

				var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(stringValue);

				prop.SetValue(settings, value, null);
			}

			return settings as ISettings;
		}
	}
}
