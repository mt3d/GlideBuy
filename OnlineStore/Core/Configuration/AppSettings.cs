using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Configuration
{
	/**
	 * App settings is basically a collection of configurations. Each configuration
	 * is represented by IConfig.
	 * 
	 * Will get injected as a service in the controllers.
	 */
	public class AppSettings
	{
		/**
		 * One one hand, you want to store all configurations in the same place.
		 * But on the other hand, each configuration is usually accessed by type,
		 * so you want to provide easy access using the type, without having
		 * to search over the whole list.
		 */
		private readonly Dictionary<Type, IConfig> configs; // no duplications allowed

		public AppSettings(IList<IConfig>? configs = null)
		{
			this.configs = configs
				?.OrderBy(config => config.GetOrder())
				?.ToDictionary(config => config.GetType(), config => config)
				?? new Dictionary<Type, IConfig>();
		}

		// Get a configuration by its type.
		public TConfig Get<TConfig>() where TConfig : class, IConfig
		{
			if (configs[typeof(TConfig)] is not TConfig config)
			{
				throw new Exception($"No configuration with type '{typeof(TConfig)}' found");
			}

			return config;
		}

		// Update the list of configurations.
		public void Update(IList<IConfig> configs)
		{
			foreach (var config in configs)
			{
				this.configs[config.GetType()] = config;
			}
		}
	}
}
