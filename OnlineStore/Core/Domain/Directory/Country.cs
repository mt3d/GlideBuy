using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Directory
{
	public class Country : BaseEntity
	{
		public string Name { get; set; }

		public bool AllowShipping { get; set; }

		public bool AllowBilling { get; set; }

		public int DisplayOrder { get; set; }
	}
}
