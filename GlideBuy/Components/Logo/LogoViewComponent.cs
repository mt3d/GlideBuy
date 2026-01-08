using GlideBuy.Models.Common;
using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components.Logo
{
	public class LogoViewComponent : ViewComponent
	{
		private readonly ICommonModelFactory _commonModelFactory;

		public LogoViewComponent(ICommonModelFactory commonModelFactory)
		{
			_commonModelFactory = commonModelFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			LogoModel model = await _commonModelFactory.PrepareLogoModelAsync();

			return View(model);
		}
	}
}
