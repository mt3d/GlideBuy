namespace GlideBuy.Infrastructure.StartupConfigurations
{
	public enum StartupOrder
	{
		Routing = 400, // before authentication
		Authentication = 500, // between UseRouting and UseEndpoints
		Endpoints = 900, // authentication before MVC
		Mvc = 1000, // should be loaded last

		/*
		 * Configure your application startup by adding app.UseAuthorization() in the application startup code. If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them. 
		 */
	}
}
