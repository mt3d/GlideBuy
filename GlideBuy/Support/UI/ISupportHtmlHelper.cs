using Microsoft.AspNetCore.Html;

namespace GlideBuy.Support.UI
{
	public interface ISupportHtmlHelper
	{
		void AddInlineScriptParts(ResourceLocation location, string script);

		public IHtmlContent GenerateInlineScripts(ResourceLocation location);
	}
}
