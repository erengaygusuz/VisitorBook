using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.RegionDtos;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class RegionRequestDtoValidator : AbstractValidator<RegionRequestDto>
    {
        private readonly IStringLocalizer<Language> _localization;

        public RegionRequestDtoValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.Name)
                .NotNull().WithMessage(_localization["Validators.Region.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Region.Message2.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.Region.Message3.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.Region.Message4.Text"].Value);
        }
    }
}
