using OnlineStore.Pages.Admin.Product;

namespace OnlineStore.Models.Localization
{
	public class Language
	{
		public int LanguageId { get; set; }

		/// <summary>
		/// A human readable name of the language.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The culture code of the language.
		/// </summary>
		public string LanguageCulture { get; set; }

		/// <summary>
		/// A short, unique code used in URLs for SEO-friendly links.
		/// Example: "en" for English, "fr" for French.
		/// Helps generate language-specific URLs: /en/products vs /fr/products.
		/// </summary>
		public string UniqueSeoCode { get; set; }

		/// <summary>
		/// The file name of the flag image representing the language.
		/// </summary>
		public string FlagImageFileName { get; set; }

		public bool Rtl { get; set; }

		public int DefaultCurrencyId { get; set; }

		public bool Published { get; set; }

		public int DisplayOrder { get; set; }
	}
}
