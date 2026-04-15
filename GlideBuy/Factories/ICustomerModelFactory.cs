using GlideBuy.Models.Customer;

namespace GlideBuy.Factories
{
    public interface ICustomerModelFactory
    {
        Task<RegisterModel> PrepareRegisterModelAsync(RegisterModel model);

        Task<RegisterResultModel> PrepareRegisterResultModelAsync(int resultId, string returnUrl);

        Task<LoginModel> PrepareLoginModelAsync(bool? checkoutAsGuest);
    }
}
