namespace GlideBuy.Models.Catalog
{
	public class CategorySimpleModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string SeName { get; set; }

		public bool IncludeInTopMenu { get; set; }

		public bool HaveSubCategories { get; set; }

		public List<CategorySimpleModel> Subcategories { get; set; } = new List<CategorySimpleModel>();

		public string IconName { get; set; }
	}
}
