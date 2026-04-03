using GlideBuy.Areas.Admin.Factories;
using GlideBuy.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Areas.Admin.Controllers
{
    public class CustomerController : BaseAdminController
    {
        protected readonly ICustomerModelFactory _customerModelFactory;

        public CustomerController(ICustomerModelFactory customerModelFactory)
        {
            _customerModelFactory = customerModelFactory;
        }

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            return View();
        }

        public virtual async Task<IActionResult> CustomerList(CustomerSearchModel searchModel)
        {
            var model = await _customerModelFactory.PrepareCustomerListModelAsync(searchModel);

            return Json(model);
        }
    }
}
