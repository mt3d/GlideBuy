using GlideBuy.Areas.Admin.Models.Plugins;
using GlideBuy.Services.Plugins;
using GlideBuy.Support.Models.Extensions;

namespace GlideBuy.Areas.Admin.Factories
{
	public class PluginModelFactory : IPluginModelFactory
	{
		private readonly IPluginService _pluginService;

		public PluginModelFactory(IPluginService pluginService)
		{
			_pluginService = pluginService;
		}

		public async Task<PluginListModel> PreparePluginListModel(PluginSearchModel searchModel)
		{
			ArgumentNullException.ThrowIfNull(searchModel);

			// TODO: Enable search using group.
			// TODO: Enable search using load mode.
			// TODO: Enable search using name.
			// TODO: Enable search using author.

			var plugins = (await _pluginService.GetPluginDescriptorsAsync<IPlugin>()).ToPagedList(searchModel);

			// TODO: Move to a dedicated method.
			var model = new PluginListModel();
			model.Data = (plugins.Select(pluginDescriptor =>
			{
				// TODO: Use automapper.
				var pluginModel = new PluginModel();
				pluginModel.Group = pluginDescriptor.Group;
				pluginModel.FriendlyName = pluginDescriptor.FriendlyName;

				return pluginModel;
			})).ToList();
			model.Draw = "1";
			model.RecordsFiltered = plugins.Count;
			model.RecordsTotal = plugins.Count;

			return model;
		}
	}
}
