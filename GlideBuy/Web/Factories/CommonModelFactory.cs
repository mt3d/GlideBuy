using GlideBuy.Core;
using GlideBuy.Models.Common;
using GlideBuy.Services.Customers;
using GlideBuy.Services.Orders;

namespace GlideBuy.Web.Factories
{
    public class CommonModelFactory : ICommonModelFactory
    {
        protected readonly IShoppingCartService _shoppingCartService;
        protected readonly ICustomerService _customerService;
        protected readonly IWorkContext _workContext;

        public CommonModelFactory(
            IShoppingCartService shoppingCartService,
            ICustomerService customerService,
            IWorkContext workContext)
        {
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
            _workContext = workContext;
        }

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

        public async Task<ShoppingCartButtonModel> PrepareCartButtonModelAsync()
        {
            var model = new ShoppingCartButtonModel();

            // TODO: Read current customer.
            if (true)
            {
                model.ShoppingCartItems = (await _shoppingCartService.GetShoppingCartAsync()).Sum(item => item.Quantity);
            }

            return model;
        }

        public virtual async Task<HeaderLinksModel> PrepareHeaderLinksModel()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            var model = new HeaderLinksModel()
            {
                IsAuthenticated = await _customerService.IsRegisteredAsync(customer),
                CustomerName = await _customerService.IsRegisteredAsync(customer) ? await _customerService.FormatUsernameAsync(customer) : string.Empty
            };

            return model;
        }
    }
}
