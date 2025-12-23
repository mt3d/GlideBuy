using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GlideBuy.Support.Controllers
{
	public class BaseController : Controller
	{
		private readonly IRazorViewEngine _viewEngine;

		public BaseController(IRazorViewEngine viewEngine)
		{
			_viewEngine = viewEngine;
		}

		protected virtual async Task<string> RenderPartialViewToStringAsync(string viewname, object model)
		{
			// needed for the ViewContext, needed for finding the view
			var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor, ModelState);

			if (string.IsNullOrEmpty(viewname))
			{
				viewname = ControllerContext.ActionDescriptor.ActionName;
			}

			ViewData.Model = model;

			// needed for the ViewContext, represents a search for view result, the found view will be rendered at last
			var viewResult = _viewEngine.FindView(actionContext, viewname, false);

			// TODO: Check for null result.

			await using var stringWriter = new StringWriter();
			var viewContext = new ViewContext(actionContext, viewResult.View, ViewData, TempData, stringWriter, new HtmlHelperOptions());

			await viewResult.View.RenderAsync(viewContext);

			return stringWriter.GetStringBuilder().ToString();
		}
	}
}
