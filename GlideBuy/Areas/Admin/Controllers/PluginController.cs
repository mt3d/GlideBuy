using GlideBuy.Areas.Admin.Factories;
using GlideBuy.Areas.Admin.Models.Plugins;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Areas.Admin.Controllers
{
	public class PluginController : Controller
	{
		private readonly IPluginModelFactory _pluginModelFactory;

		public PluginController(IPluginModelFactory pluginModelFactory)
		{
			_pluginModelFactory = pluginModelFactory;
		}

		[Area("Admin")]
		public IActionResult List()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LoadList(PluginSearchModel saerchModel)
		{
			var model = await _pluginModelFactory.PreparePluginListModel(saerchModel);

			return Json(model);
		}
	}
}
