
namespace GlideBuy.Services.Plugins
{
	public class PluginService : IPluginService
	{
		public async Task<IList<PluginDescriptor>> GetPluginDescriptorsAsync<TPlugin>() where TPlugin : class, IPlugin
		{
			throw new NotImplementedException();
		}

		public Task<IList<TPlugin>> GetPluginsAsync<TPlugin>() where TPlugin : class, IPlugin
		{
			throw new NotImplementedException();
		}
	}
}
