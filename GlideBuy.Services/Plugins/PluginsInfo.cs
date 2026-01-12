
namespace GlideBuy.Services.Plugins
{
	public class PluginsInfo : IPluginsInfo
	{
		protected IList<PluginDescriptorBaseInfo> _installedPlugins = new List<PluginDescriptorBaseInfo>();

		public IList<PluginDescriptorBaseInfo> InstalledPlugins
		{
			get => throw new NotImplementedException();
			set => _installedPlugins = value;
		}

		public void LoadPluginInfo()
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			throw new NotImplementedException();
		}

		public Task SaveAsync()
		{
			throw new NotImplementedException();
		}

		public IList<(PluginDescriptor pluginDescriptor, bool needToDeploy)> PluginDescriptors { get; set; }
	}
}
