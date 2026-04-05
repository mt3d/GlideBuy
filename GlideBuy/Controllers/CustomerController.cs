using GlideBuy.Core;
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
        protected readonly IWorkContext _workContext;

        public CustomerController(
            CustomerSettings customerSettings,
            ICustomerModelFactory customerModelFactory,
            IWorkContext workContext)
        {
            _customerSettings = customerSettings;
            _customerModelFactory = customerModelFactory;
            _workContext = workContext;
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

        [HttpPost]
        public virtual async Task<IActionResult> Register(
            RegisterModel model,
            string returnUrl,
            bool captchaValid,
            IFormCollection form)
        {
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
            {
                return RedirectToRoute("RegisterResult");
            }

            var customer = await _workContext.GetCurrentCustomerAsync();

            // TODO: 1. Check if user already registered

            // TODO: Handle captcha

            // TODO: Handle GDPR

            // TODO: Handle registration

            if (ModelState.IsValid)
            {
                // Try register the customer
                var customerUserName = model.Username;
                var customerEmail = model.Email;

                // If the customer is approved, it will become active immediately.
                var isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;

                // Handle newsletter

                // Handle GDPR

                // Handle notifications

                // Complete registration based on registration type
            }

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
