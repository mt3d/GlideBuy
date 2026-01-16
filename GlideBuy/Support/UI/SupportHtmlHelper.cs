using Microsoft.AspNetCore.Html;
using System.Text;

namespace GlideBuy.Support.UI
{
	public class SupportHtmlHelper : ISupportHtmlHelper
	{
		private readonly Dictionary<ResourceLocation, List<string>> _inlineScriptParts = new();

		public void AddInlineScriptParts(ResourceLocation location, string script)
		{
			if (!_inlineScriptParts.ContainsKey(location))
			{
				_inlineScriptParts.Add(location, new());
			}

			if (string.IsNullOrEmpty(script))
			{
				return;
			}

			if (_inlineScriptParts[location].Contains(script))
			{
				return;
			}

			_inlineScriptParts[location].Add(script);
		}

		public IHtmlContent GenerateInlineScripts(ResourceLocation location)
		{
			// TODO: Why check for null?
			if (!_inlineScriptParts.TryGetValue(location, out var value) || value == null)
			{
				return HtmlString.Empty;
			}

			var result = new StringBuilder();
			foreach (var item in value)
			{
				result.Append(item);
				result.Append(Environment.NewLine);
			}

			return new HtmlString(result.ToString());
		}
	}
}
