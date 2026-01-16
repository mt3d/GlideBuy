namespace GlideBuy.Areas.Admin.Models.Plugins
{
	public class PluginModel
	{
		public string Group { get; set; }

		public string FriendlyName { get; set; }

		public bool Installed { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsActive { get; set; }
	}
}
