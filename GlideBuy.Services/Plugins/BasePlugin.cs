namespace GlideBuy.Services.Plugins
{
	public class BasePlugin : IPlugin
	{
		public virtual PluginDescriptor PluginDescriptor { get; set; }

		public virtual Task InstallAsync()
		{
			return Task.CompletedTask;
		}

		public virtual Task UninstallAsync()
		{
			return Task.CompletedTask;
		}
	}
}
