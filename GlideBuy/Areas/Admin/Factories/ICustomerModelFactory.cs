using GlideBuy.Areas.Admin.Models.Customers;

namespace GlideBuy.Areas.Admin.Factories
{
    public interface ICustomerModelFactory
    {
        Task<CustomerListModel> PrepareCustomerListModelAsync(CustomerSearchModel searchModel);
    }
}
