namespace GlideBuy.Core.Domain.Directory
{
	// TODO: Add to the DbContext.
	public class Currency
	{
		public string Name { get; set; }

		public string CurrencyCode { get; set; }

		public decimal Rate { get; set; }

		public string DisplayLocale { get; set; }

		public bool Published { get; set; }

		public int DisplayOrder { get; set; }

		public DateTime CreatedOnUtc { get; set; }

		public DateTime UpdatedOnUtc { get; set; }

		// TODO: Add rounding type.
	}
}
