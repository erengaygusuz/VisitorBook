using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class CityViewModelValidator : AbstractValidator<CityViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public CityViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization; 

            RuleFor(x => x.City.Name)
                .NotNull().WithMessage(_localization["Validators.City.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.City.Message2.Text"].Value)
                .MinimumLength(2).WithMessage(_localization["Validators.City.Message3.Text"].Value)
                .MaximumLength(50).WithMessage(_localization["Validators.City.Message4.Text"].Value);

            RuleFor(x => x.City.Code)
                .NotNull().WithMessage(_localization["Validators.City.Message5.Text"].Value)
                .MinimumLength(2).WithMessage(_localization["Validators.City.Message6.Text"].Value)
                .MaximumLength(50).WithMessage(_localization["Validators.City.Message7.Text"].Value);

            RuleFor(x => x.City.CountryId)
               .Must(IsCountrySelected).WithMessage(_localization["Validators.City.Message8.Text"].Value);
        }

        private bool IsCountrySelected(int countryId)
        {
            if (countryId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
