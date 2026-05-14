using ArabRiver.Service.DTOs.Contact;
using FluentValidation;

namespace ArabRiver.Service.Validations.Contact
{
    public class CreateContactMessageDtoValidator
    : AbstractValidator<CreateContactMessageDto>
    {
        public CreateContactMessageDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(3000);
        }
    }
}
