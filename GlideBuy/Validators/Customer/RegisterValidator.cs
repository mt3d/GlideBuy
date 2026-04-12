using FluentValidation;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Models.Customer;
using GlideBuy.Support.Validators;

namespace GlideBuy.Validators.Customer
{
    public class RegisterValidator : BaseValidator<RegisterModel>
    {
        public RegisterValidator(
            CustomerSettings customerSettings)
        {
            // TODO: Localize the messages

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            // TODO: Check rule for valid email

            if (customerSettings.EnteringEmailTwice)
            {
                RuleFor(x => x.ConfirmEmail).NotEmpty().WithMessage("Confirm email is required");
                // TODO: Check rule for valid email

                RuleFor(x => x.ConfirmEmail).Equal(x => x.Email).WithMessage("The emails do not match");
            }

            if (customerSettings.UsernameEnabled)
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");

                // TODO: Check rule for valid username
            }

            // TODO: Check rule for password
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm passwor is required");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The passwords do not match");
        }
    }
}
