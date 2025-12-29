using GlideBuy.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlideBuy.Web.Models.Catalog
{
	public class CatalogProductsModel
	{
		public string NoResultMessage { get; set; }

		public bool AllowProductSorting { get; set; }

		public IList<SelectListItem> AvailableSortOptions { get; set; } = new List<SelectListItem>();

		public bool AllowCustomerToSelectPageSize { get; set; }

		public IList<SelectListItem> PageSizeOptions { get; set; } = new List<SelectListItem>();

		public IList<ProductOverviewModel> Products { get; set; } = new List<ProductOverviewModel>();
	}
}
