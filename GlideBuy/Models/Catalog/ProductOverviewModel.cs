using GlideBuy.Models.Media;

namespace GlideBuy.Models.Catalog
{
	public class ProductOverviewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string ShortDescription { get; set; }

		public string FullDescription { get; set; }

		public IList<PictureModel> PictureModels { get; set; }
	}
}
