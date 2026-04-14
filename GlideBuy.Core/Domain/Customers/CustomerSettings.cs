using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Domain.Customers
{
    public class CustomerSettings : ISettings
    {
        // TODO: Remove the default value from here.
        public UserRegistrationType UserRegistrationType { get; set; } = UserRegistrationType.Standard;

        public bool AcceptPrivacyPolicyEnabled { get; set; }

        public bool UsernameEnabled { get; set; }

        public PasswordFormat DefaultPasswordFormat { get; set; }

        public bool EnteringEmailTwice { get; set; }

        // TODO: Remove the default value from here.
        public CustomerNameFormat CustomerNameFormat { get; set; } = CustomerNameFormat.ShowFullNames;
    }
}
