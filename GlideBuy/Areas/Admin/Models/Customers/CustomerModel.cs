using GlideBuy.Support.Models;

namespace GlideBuy.Areas.Admin.Models.Customers
{
    public record CustomerModel : BaseEntityModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
