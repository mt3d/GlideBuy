using GlideBuy.Models.Customer;

namespace GlideBuy.Factories
{
    public interface ICustomerModelFactory
    {
        Task<RegisterModel> PrepareRegisterModelAsync(RegisterModel model);
    }
}
