namespace GlideBuy.Services.Plugins
{
	public interface IPluginManager<TPlugin> where TPlugin : class, IPlugin
	{
		Task<IList<TPlugin>> LoadAllPluginsAsync();

		Task<IList<TPlugin>> LoadActivePluginsAsync(List<string> systemNames);

		Task<TPlugin> LoadPluginBySystemName(string systemName);

		bool IsPluginActive(TPlugin plugin, List<string> systemNames);
	}
}
