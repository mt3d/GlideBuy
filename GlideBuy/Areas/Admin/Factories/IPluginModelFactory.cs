using GlideBuy.Areas.Admin.Models.Plugins;

namespace GlideBuy.Areas.Admin.Factories
{
	public interface IPluginModelFactory
	{
		Task<PluginListModel> PreparePluginListModel(PluginSearchModel searchModel);
	}
}
