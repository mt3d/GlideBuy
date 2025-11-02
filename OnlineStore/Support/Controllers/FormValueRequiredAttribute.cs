using GlideBuy.Support.Extensions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace GlideBuy.Support.Controllers
{
	public sealed class FormValueRequiredAttribute : ActionMethodSelectorAttribute
	{
		private readonly string[] _submitButtonNames;

		public FormValueRequiredAttribute(params string[] submitButtonNames)
		{
			_submitButtonNames = submitButtonNames;
		}

		public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
		{
			if (!routeContext.HttpContext.Request.IsPostRequest())
			{
				return false;
			}

			var form = routeContext.HttpContext.Request.Form;

			foreach (var buttonName in _submitButtonNames)
			{
				try
				{
					// TODO: Implement different requirements ('Equal' and 'StartsWith')

					if (form.Keys.Any(x => x.Equals(buttonName, StringComparison.InvariantCultureIgnoreCase)))
					{
						return true;
					}
				}
				catch
				{
					// Just to ensure no errors are thrown.
				}
			}

			return false;
		}
	}
}
