using FluentValidation;

namespace GlideBuy.Support.Validators
{
    public class BaseValidator<TModel> : AbstractValidator<TModel> where TModel : class
    {
    }
}
