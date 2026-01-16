namespace GlideBuy.Support.Models.DataTables
{
	public class DataUrl
	{
		public DataUrl(string action, string controller, RouteValueDictionary? routeValues)
		{
			ActionName = action;
			ControllerName = controller;
		}

		public string Url { get; set; }

		public string ActionName { get; set; }

		public string ControllerName { get; set; }
	}
}
