using GlideBuy.Support.Models;
using System.ComponentModel.DataAnnotations;

namespace GlideBuy.Models.Customer
{
    public record RegisterModel : BaseModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EnterEmailTwice { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ConfirmEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool AcceptPrivacyPolicyEnabled { get; set; }

        public bool UsernameEnabled { get; set; }

        public string Username { get; set; }
    }
}
