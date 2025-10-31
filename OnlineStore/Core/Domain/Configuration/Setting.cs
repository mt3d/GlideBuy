using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Configuration
{
	public class Setting : BaseEntity
	{
		public Setting() { }

		public Setting(string name, string value)
		{
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Key format: OrderSettings.MinOrderTotalAmount
		/// </summary>
		public string Name { get; set; }

		public string Value { get; set; }

		//public int StoreId { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
