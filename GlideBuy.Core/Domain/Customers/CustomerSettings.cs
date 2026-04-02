using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Domain.Customers
{
    public class CustomerSettings : ISettings
    {
        public UserRegistrationType UserRegistrationType { get; set; }

        public bool AcceptPrivacyPolicyEnabled { get; set; }
    }
}
