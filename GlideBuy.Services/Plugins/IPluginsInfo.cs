namespace GlideBuy.Services.Plugins
{
	/// <summary>
	/// Represents all the information stored in the system about plugins.
	/// </summary>
	public interface IPluginsInfo
	{
		/// <summary>
		/// Saves the info to the file.
		/// </summary>
		void Save();

		Task SaveAsync();

		void LoadPluginInfo();

		IList<PluginDescriptorBaseInfo> InstalledPlugins { get; set; }

		IList<(PluginDescriptor pluginDescriptor, bool needToDeploy)> PluginDescriptors { get; set; }
	}
}
