using GlideBuy.Areas.Admin.Models.Plugins;

namespace GlideBuy.Areas.Admin.Factories
{
	public class PluginModelFactory : IPluginModelFactory
	{
		public async Task<PluginListModel> PreparePluginListModel(PluginSearchModel searchModel)
		{
			// TODO: Enable search using group.
			// TODO: Enable search using load mode.
			// TODO: Enable search using name.
			// TODO: Enable search using author.

			var model = new PluginListModel();

			return model;
		}
	}
}
