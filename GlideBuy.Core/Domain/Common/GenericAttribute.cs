using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Common
{
	public class GenericAttribute : BaseEntity
	{
		public int EntityId { get; set; }

		// Group entities by type name (Customer, Order...)
		public string KeyGroup { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public DateTime? CreatedOrUpdatedAtUtc { get; set; }
	}
}
