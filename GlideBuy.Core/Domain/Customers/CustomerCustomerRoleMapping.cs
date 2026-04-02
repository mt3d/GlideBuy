using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Customers
{
    public class CustomerCustomerRoleMapping : BaseEntity
    {
        public int CustomerId { get; set; }

        public int CustomerRoleId { get; set; }
    }
}
