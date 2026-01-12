
namespace GlideBuy.Services.Plugins
{
	public class PluginManager<TPlugin> : IPluginManager<TPlugin> where TPlugin : class, IPlugin
	{
		private readonly IPluginService _pluginService;

		public PluginManager(IPluginService pluginService)
		{
			_pluginService = pluginService;
		}

		public bool IsPluginActive(TPlugin plugin, List<string> systemNames)
		{
			throw new NotImplementedException();
		}

		public Task<IList<TPlugin>> LoadActivePluginsAsync(List<string> systemNames)
		{
			throw new NotImplementedException();
		}

		public Task<IList<TPlugin>> LoadAllPluginsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<TPlugin> LoadPluginBySystemName(string systemName)
		{
			throw new NotImplementedException();
		}
	}
}
