using GlideBuy.Core;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Core.Domain.Directory;
using GlideBuy.Core.Http;
using GlideBuy.Core.Security;
using GlideBuy.Services.Customers;

namespace GlideBuy.Support
{
	public class WebWorkContext : IWorkContext
	{
		private Customer? _cachedCustomer;

		private readonly ICustomerService _customerService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly CookieSettings _cookieSettings;

		public WebWorkContext(
			ICustomerService customerService,
			IHttpContextAccessor httpContextAccessor,
			CookieSettings cookieSettings)
		{
			_customerService = customerService;
			_httpContextAccessor = httpContextAccessor;
			_cookieSettings = cookieSettings;
		}

		private void SetCustomerCookie(Guid customerGuid)
		{
			if (_httpContextAccessor.HttpContext?.Response.HasStarted ?? true)
			{
				return;
			}

			// Delete the current cookie.
			var cookieName = $"{CookieDefaults.Prefix}{CookieDefaults.CustomerCookie}";
			_httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

			var cookieExpiration = _cookieSettings.CustomerCookieExpiresHours;
			var cookieExpirationDate = DateTime.Now.AddHours(cookieExpiration);

			if (customerGuid == Guid.Empty)
			{
				// Set cookie as expired
				cookieExpirationDate = DateTime.Now.AddMonths(-1);
			}

			var options = new CookieOptions
			{
				HttpOnly = true,
				Expires = cookieExpirationDate,
				Secure = false // TODO: Check if the connection is secure
			};

			_httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, customerGuid.ToString(), options);
		}

		private string? GetCustomerCookie()
		{
			var cookieName = $"{CookieDefaults.Prefix}{CookieDefaults.CustomerCookie}";
			return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
		}

		public async Task<Customer> GetCurrentCustomerAsync()
		{
			if (_cachedCustomer != null)
			{
				return _cachedCustomer;
			}

			await SetCurrentCustomerAsync();

			return _cachedCustomer!;
		}

		public async Task SetCurrentCustomerAsync(Customer? customer = null)
		{
			if (customer == null)
			{
				// TODO: Check for a background task user

				// TODO: Check for  a search engine user

				// TODO: Handle registered users

				// TODO: Handle impersonated user

				// Get guest customer
				if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
				{
					var customerCookie = GetCustomerCookie();

					if (Guid.TryParse(customerCookie, out var customerGuid))
					{
						var customerByCookie = await _customerService.GetCustomerByGuidAsync(customerGuid);

						if (customerByCookie != null) // TODO: Check that the user isn't registered
						{
							customer = customerByCookie;
						}
					}
				}

				// If the previous step failed, create a guest customer.
				if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
				{
					customer = await _customerService.InsertGuestCustomerAsync();
				}
			}

			if (!customer.Deleted && customer.Active && !customer.RequireReLogin)
			{
				SetCustomerCookie(customer.CustomerGuid);

				_cachedCustomer = customer;
			}
		}

		// TODO: Implement in the future when multicurrencies are fully-supported.
		public async Task<Currency> GetWorkingCurrencyAsync()
		{
			return new Currency
			{
				Name = "US Dollar",
				CurrencyCode = "USD",
				Rate = 1,
				DisplayLocale = "en-US",
				Published = true,
				DisplayOrder = 1,
				CreatedOnUtc = DateTime.UtcNow,
				UpdatedOnUtc = DateTime.UtcNow,
			};
		}
	}
}
