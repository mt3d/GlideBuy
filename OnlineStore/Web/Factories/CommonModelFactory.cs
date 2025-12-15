using GlideBuy.Models.Common;

namespace GlideBuy.Web.Factories
{
	public class CommonModelFactory : ICommonModelFactory
	{
		public async Task<LogoModel> PrepareLogoModelAsync()
		{
			// TODO: Get the current store, then get the localized name of it.

			var model = new LogoModel()
			{
				StoreName = "GlideBuy",
				LogoPath = "/images/logo.png"
			};

			return model;
		}
	}
}
