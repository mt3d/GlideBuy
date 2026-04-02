using GlideBuy.Core.Domain.Customers;
using GlideBuy.Factories;
using GlideBuy.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Controllers
{
    public class CustomerController : Controller
    {
        protected readonly CustomerSettings _customerSettings;
        protected readonly ICustomerModelFactory _customerModelFactory;

        public CustomerController(
            CustomerSettings customerSettings,
            ICustomerModelFactory customerModelFactory)
        {
            _customerSettings = customerSettings;
            _customerModelFactory = customerModelFactory;
        }

        public virtual async Task<IActionResult> Register(string returnUrl)
        {
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
            {
                return RedirectToRoute("RegisterResult");
            }    

            var model = new RegisterModel();
            model = await _customerModelFactory.PrepareRegisterModelAsync(model);

            return View(model);
        }

        // TODO: WIP
        public virtual async Task<IActionResult> RegisterResult(int resultId, string returnUrl)
        {
            // TODO: Check the return URL

            // TODO: Prepare the register model using the result ID
            return View();
        }
    }
}
