using GlideBuy.Support.Models;

namespace GlideBuy.Areas.Admin.Models.Plugins
{
	public record PluginModel : BaseModel
	{
		public string Group { get; set; }

		public string FriendlyName { get; set; }

		public bool Installed { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsActive { get; set; }
	}
}
