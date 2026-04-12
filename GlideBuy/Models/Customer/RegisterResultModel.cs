using GlideBuy.Support.Models;

namespace GlideBuy.Models.Customer
{
    public record RegisterResultModel : BaseModel
    {
        public string Result { get; set; }

        public string ReturnUrl { get; set; }
    }
}
