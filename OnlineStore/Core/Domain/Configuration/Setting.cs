namespace GlideBuy.Core.Domain.Configuration
{
	public class Setting
	{
		public Setting() { }

		public Setting(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public int SettingId { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		//public int StoreId { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
