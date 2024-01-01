using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class CountryViewModelValidator : AbstractValidator<CountryViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public CountryViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.Country.Name)
                .NotNull().WithMessage(_localization["Validators.Country.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Country.Message2.Text"].Value)
                .MinimumLength(2).WithMessage(_localization["Validators.Country.Message3.Text"].Value)
                .MaximumLength(50).WithMessage(_localization["Validators.Country.Message4.Text"].Value);

            RuleFor(x => x.Country.Code)
                .NotNull().WithMessage(_localization["Validators.Country.Message5.Text"].Value)
                .MinimumLength(2).WithMessage(_localization["Validators.Country.Message6.Text"].Value)
                .MaximumLength(50).WithMessage(_localization["Validators.Country.Message7.Text"].Value);

            RuleFor(x => x.Country.SubRegionId)
               .Must(IsSubRegionSelected).WithMessage(_localization["Validators.Country.Message8.Text"].Value);
        }

        private bool IsSubRegionSelected(int subRegionId)
        {
            if (subRegionId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
