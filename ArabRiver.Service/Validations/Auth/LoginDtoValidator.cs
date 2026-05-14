using ArabRiver.Service.DTOs.Auth;
using FluentValidation;

namespace ArabRiver.Service.Validations.Auth
{
    public class LoginDtoValidator
    : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
