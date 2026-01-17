namespace GlideBuy.Services.Plugins
{
	public interface IPlugin
	{
		PluginDescriptor PluginDescriptor { get; set; }

		Task InstallAsync();

		Task UninstallAsync();
	}
}
