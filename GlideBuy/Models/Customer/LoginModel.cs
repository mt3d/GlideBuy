using GlideBuy.Support.Models;

namespace GlideBuy.Models.Customer
{
    public record LoginModel : BaseModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        // TODO: Handle username and registration type

        // TODO: Handle captcha

        public bool CheckoutAsGuest { get; set; }
    }
}
