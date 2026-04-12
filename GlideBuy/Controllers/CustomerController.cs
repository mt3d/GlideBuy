using GlideBuy.Core;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Factories;
using GlideBuy.Models.Customer;
using GlideBuy.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Controllers
{
    public class CustomerController : Controller
    {
        protected readonly CustomerSettings _customerSettings;
        protected readonly ICustomerModelFactory _customerModelFactory;
        protected readonly IWorkContext _workContext;
        protected readonly ICustomerRegistrationService _customerRegistrationService;

        public CustomerController(
            CustomerSettings customerSettings,
            ICustomerModelFactory customerModelFactory,
            IWorkContext workContext,
            ICustomerRegistrationService customerRegistrationService)
        {
            _customerSettings = customerSettings;
            _customerModelFactory = customerModelFactory;
            _workContext = workContext;
            _customerRegistrationService = customerRegistrationService;
        }

        public virtual async Task<IActionResult> Register(string? returnUrl)
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
            string? returnUrl,
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
                var customerUsername = model.Username;
                var customerEmail = model.Email;

                // If the customer is approved, it will become active immediately.
                var isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new CustomerRegistrationRequest
                {
                    Customer = customer,
                    Email = customerEmail,
                    Username = _customerSettings.UsernameEnabled ? customerUsername : customerEmail,
                    Password = model.Password,
                    PasswordFormat = _customerSettings.DefaultPasswordFormat,
                    StoreId = 1, // TODO: Handle store ID
                    IsApproved = isApproved,
                };

                var registrationResult = await _customerRegistrationService.RegisterCustomerAsync(registrationRequest);
                if (registrationResult.Success)
                {
                    // TODO: Handle time zone

                    // TODO: Handle VAT

                    // TODO: Handle form fields

                    // Handle newsletter

                    // Handle Privacy Policy

                    // Handle GDPR

                    // Handle notifications

                    // Complete registration based on registration type
                    switch (_customerSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.Standard:
                            // TODO: Send a welcome message
                            // TODO: Publish event

                            returnUrl = Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.Standard, returnUrl });
                            // TODO: Sign in the customer
                            return await _customerRegistrationService.SignInCustomerAsync(customer, returnUrl, true);
                        default:
                            return RedirectToRoute("Homepage");
                    }
                }

                foreach (var error in registrationResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            model = await _customerModelFactory.PrepareRegisterModelAsync(model);

            return View(model);
        }

        // TODO: WIP
        public virtual async Task<IActionResult> RegisterResult(int resultId, string? returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = Url.RouteUrl("Homepage");
            }

            var model = await _customerModelFactory.PrepareRegisterResultModelAsync(resultId, returnUrl);

            return View(model);
        }
    }
}
