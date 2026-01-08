using GlideBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GlideBuy.Support.Mvc.Routing
{
	public class CustomUrlHelper : ICustomUrlHelper
	{
		private readonly IUrlHelperFactory _urlHelperFactory;
		private readonly IActionContextAccessor _actionContextAccessor;

		public CustomUrlHelper(
			IUrlHelperFactory urlHelperFactory,
			IActionContextAccessor actionContextAccessor)
		{
			_urlHelperFactory = urlHelperFactory;
			_actionContextAccessor = actionContextAccessor;
		}

		/// <summary>
		/// Similar to RouteUrl but supports generic urls.
		/// </summary>
		public async Task<string?> RouteGenericUrlAsync<T>(object? values = null)
		{
			var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

			return typeof(T) switch
			{
				var entityType when entityType == typeof(Category)
					=> urlHelper.RouteUrl("Category", values),
				var entityType => urlHelper.RouteUrl(entityType.Name, values)
			};
		}
	}
}
