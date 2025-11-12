using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Common
{
	public class Address : BaseEntity
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string Company { get; set; }

		public string Country { get; set; }
	}
}
