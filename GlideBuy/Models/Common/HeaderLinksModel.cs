using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Models.Common
{
    public class HeaderLinksModel
    {
        public bool IsAuthenticated { get; set; }
        public UserRegistrationType RegistrationType { get; set; }
        public string CustomerName { get; set; }
    }
}
