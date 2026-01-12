namespace GlideBuy.Services.Plugins
{
	public interface IPluginService
	{
		// TODO: Specify loading mode.
		Task<IList<TPlugin>> GetPluginsAsync<TPlugin>() where TPlugin : class, IPlugin;
	}
}
