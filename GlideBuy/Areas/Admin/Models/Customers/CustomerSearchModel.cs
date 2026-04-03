using GlideBuy.Support.Models;

namespace GlideBuy.Areas.Admin.Models.Customers
{
    public record CustomerSearchModel : BaseSearchModel
    {
        public string SearchEmail { get; set; }

        public string SearchUsername { get; set; }
    }
}
