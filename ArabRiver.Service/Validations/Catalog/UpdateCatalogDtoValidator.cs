using ArabRiver.Service.DTOs.Catalog;
using FluentValidation;

namespace ArabRiver.Service.Validations.Catalog
{
    public class UpdateCatalogDtoValidator
     : AbstractValidator<UpdateCatalogDto>
    {
        public UpdateCatalogDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.GoogleDriveLink)
                .NotEmpty()
                .Must(link =>
                    Uri.TryCreate(link,
                    UriKind.Absolute,
                    out _))
                .WithMessage("Invalid URL format");

            RuleFor(x => x.Description)
                .MaximumLength(5000);

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0);
        }
    }
}
