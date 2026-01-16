using GlideBuy.Support.UI;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace GlideBuy.Support.TagHelpers
{
	// TODO: Split into TagHelperExtensions and HtmlExtensions.
	public static class HtmlExtensions
	{
		// TODO: Study. Move out of here.
		public static async Task<string> RenderHtmlContentAsync(this IHtmlContent htmlContent)
		{
			await using var writer = new StringWriter();
			htmlContent.WriteTo(writer, HtmlEncoder.Default);
			return writer.ToString();
		}

		public static async Task<IDictionary<string, string>> GetAttributeDictionaryAsync(this TagHelperOutput output)
		{
			ArgumentNullException.ThrowIfNull(output);

			var result = new Dictionary<string, string>();

			if (!output.Attributes.Any())
				return result;

			foreach (var attrName in output.Attributes.Select(x => x.Name).Distinct())
			{
				result.Add(attrName, await output.GetAttributeValueAsync(attrName));
			}

			return result;
		}

		public static async Task<string> GetAttributeValueAsync(this TagHelperOutput output, string attributeName)
		{

			if (string.IsNullOrEmpty(attributeName) || !output.Attributes.TryGetAttribute(attributeName, out var attr))
				return null;

			if (attr.Value is string stringValue)
				return stringValue;

			return attr.Value switch
			{
				HtmlString htmlString => htmlString.ToString(),
				IHtmlContent content => await content.RenderHtmlContentAsync(),
				_ => default
			};
		}
	}

	/**
	 * UrlResolutionTagHelper:
	 * Resolves URLs starting with '~/' (relative to the application's 'webroot' setting)
	 * that are not targeted by other ITagHelpers. Runs prior to other ITagHelpers to
	 * ensure application-relative URLs are resolved.
	 */
	[HtmlTargetElement("script", Attributes = "asp-location")]
	public class SupportScriptTagHelper : UrlResolutionTagHelper
	{
		private readonly IUrlHelperFactory _urlHelperFactory;
		private readonly ISupportHtmlHelper _supportHtmlHelper;

		public SupportScriptTagHelper(
			IUrlHelperFactory urlHelperFactory,
			HtmlEncoder htmlEncoder,
			ISupportHtmlHelper supportHtmlHelper) : base(urlHelperFactory, htmlEncoder)
		{
			_urlHelperFactory = urlHelperFactory;
			_supportHtmlHelper = supportHtmlHelper;
		}

		// TODO: Study.
		private static async Task<string> BuildInlineScriptTagAsync(TagHelperOutput output)
		{
			var scriptTag = new TagBuilder("script");

			var childContent = await output.GetChildContentAsync();
			var script = childContent.GetContent();

			if (!string.IsNullOrEmpty(script))
				scriptTag.InnerHtml.SetHtmlContent(new HtmlString(script));

			scriptTag.MergeAttributes(await output.GetAttributeDictionaryAsync(), replaceExisting: false);

			return await scriptTag.RenderHtmlContentAsync() + Environment.NewLine;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (Src is null)
			{
				_supportHtmlHelper.AddInlineScriptParts(Location, await BuildInlineScriptTagAsync(output));
			}

			output.SuppressOutput();
		}

		[HtmlAttributeName("asp-location")]
		public ResourceLocation Location { get; set; }

		[HtmlAttributeName("src")]
		public string? Src { get; set; }
	}
}
