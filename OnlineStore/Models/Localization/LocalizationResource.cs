namespace GlideBuy.Models.Localization
{
	public class LocalizationResource
	{
		public int LocalizationResourceId { get; set; }

		/**
		 * It is considered a best practice to store both the foreign key
		 * and the navigation property.
		 * 
		 * One can query/filter easily with CustomerId without joining, and still
		 * navigate to Customer.
		 * Clearer model, less confusion about shadow properties.
		 */
		public int LanguageId { get; set; }

		public Language Language { get; set; }

		public string ResourceName { get; set; }

		public string ResourceValue { get; set; }
	}
}
