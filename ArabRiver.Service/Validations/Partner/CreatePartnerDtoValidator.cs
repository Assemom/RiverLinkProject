using ArabRiver.Service.DTOs.Partner;
using FluentValidation;

namespace ArabRiver.Service.Validations.Partner
{
    public class CreatePartnerDtoValidator
        : AbstractValidator<CreatePartnerDto>
    {
        public CreatePartnerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
