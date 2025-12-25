using Microsoft.AspNetCore.Html;

namespace GlideBuy.Core.Infrastructure.Localization
{
	public class LocalizedString : HtmlString
	{
		public string Text { get; }

		public LocalizedString(string localizedString) : base(localizedString)
		{
			Text = localizedString;
		}
	}
}
