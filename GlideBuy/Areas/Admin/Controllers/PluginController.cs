using GlideBuy.Areas.Admin.Factories;
using GlideBuy.Areas.Admin.Models.Plugins;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class PluginController : Controller
	{
		private readonly IPluginModelFactory _pluginModelFactory;

		public PluginController(IPluginModelFactory pluginModelFactory)
		{
			_pluginModelFactory = pluginModelFactory;
		}

		public IActionResult List()
		{
			return View();
		}

		/**
		 * ASP.NET Core creates the PluginSearchModel instance through standard model binding pipeline.
		 * 
		 * When the POST request arrives, the framework examines the action method signature
		 * and sees a single complex parameter named searchModel. It then attempts to populate
		 * this object by matching incoming form keys to property names on PluginSearchModel.
		 * Properties such as SearchFriendlyName, SearchGroup, SearchLoadModeId, and SearchAuthor
		 * are filled directly from the corresponding request parameters added by NopCommerce.
		 * At the same time, properties inherited from BaseSearchModel or similar base classes,
		 * such as Draw, Start, Length, OrderBy, or paging related fields, are populated from
		 * the standard DataTables parameters. Because DataTables uses naming conventions that
		 * ASP.NET Core understands, including indexed keys for collections, the binder can
		 * also reconstruct complex properties like sorting and column definitions if they are exposed on the model.
		 */
		[HttpPost]
		public async Task<IActionResult> LoadList(PluginSearchModel searchModel)
		{
			var model = await _pluginModelFactory.PreparePluginListModel(searchModel);

			return Json(model);
		}
	}
}
