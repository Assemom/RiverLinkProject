using ArabRiver.Service.DTOs.Lead;
using FluentValidation;

namespace ArabRiver.Service.Validations.Lead
{
    public class CreateOutsideEgyptLeadDtoValidator
        : AbstractValidator<CreateOutsideEgyptLeadDto>
    {
        public CreateOutsideEgyptLeadDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.OrganizationName)
                .MaximumLength(200);
        }
    }
}
