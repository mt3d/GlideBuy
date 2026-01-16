
using GlideBuy.Core.Infrastructure;

namespace GlideBuy.Services.Plugins
{
	public class PluginService : IPluginService
	{
		private readonly IPluginsInfo _pluginsInfo;

		public PluginService()
		{
			_pluginsInfo = Singleton<PluginsInfo>.Instance;
		}

		// TODO: Support filtering.
		public async Task<IList<PluginDescriptor>> GetPluginDescriptorsAsync<TPlugin>() where TPlugin : class, IPlugin
		{
			var pluginDescriptors = _pluginsInfo.PluginDescriptors.Select(d => d.pluginDescriptor).ToList();

			// TODO: Support filtering by passed type TPlugin.

			// TODO: Order by group name or friendly name.

			return pluginDescriptors;
		}

		public Task<IList<TPlugin>> GetPluginsAsync<TPlugin>() where TPlugin : class, IPlugin
		{
			throw new NotImplementedException();
		}
	}
}
