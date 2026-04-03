using GlideBuy.Areas.Admin.Models.Customers;
using GlideBuy.Services.Customers;
using GlideBuy.Services.Plugins;

namespace GlideBuy.Areas.Admin.Factories
{
    public class CustomerModelFactory : ICustomerModelFactory
    {
        protected readonly ICustomerService _customerService;

        public CustomerModelFactory(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public virtual async Task<CustomerListModel> PrepareCustomerListModelAsync(CustomerSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);

            var customers = await _customerService.GetAllCustomersAsync();

            var model = new CustomerListModel();
            model.Data = (customers.Select(c =>
            {
                var customerModel = new CustomerModel
                {
                    Id = c.Id,
                    Email = c.Email
                };
                return customerModel;
            })).ToList();
            model.Draw = "1";
            model.RecordsFiltered = customers.TotalCount;
            model.RecordsTotal = customers.TotalCount;

            return model;
        }
    }
}
