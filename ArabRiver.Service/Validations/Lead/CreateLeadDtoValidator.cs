using ArabRiver.Service.DTOs.Lead;
using FluentValidation;

namespace ArabRiver.Service.Validations.Lead
{
    public class CreateLeadDtoValidator
    : AbstractValidator<CreateLeadDto>
    {
        public CreateLeadDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MinimumLength(7)
                .MaximumLength(20);

            RuleFor(x => x.OrganizationName)
                .MaximumLength(200);

            RuleFor(x => x.CatalogId)
                .NotEmpty();
        }
    }
}
