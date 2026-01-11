namespace GlideBuy.Services.Plugins
{
	public interface IPlugin
	{
		PluginDescriptor PluginDescriptor { get; }

		Task InstallAsync();

		Task UninstallAsync();
	}
}
