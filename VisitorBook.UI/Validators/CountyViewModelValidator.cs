using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Extensions;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class CountyViewModelValidator : AbstractValidator<CountyViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public CountyViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.County.Name)
                .NotNull().WithMessage(_localization["Validators.County.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.County.Message2.Text"].Value)
                .MinimumLength(2).WithMessage(_localization["Validators.County.Message3.Text"].Value)
                .MaximumLength(50).WithMessage(_localization["Validators.County.Message4.Text"].Value);

            RuleFor(x => x.County.Latitude)
                .NotNull().WithMessage(_localization["Validators.County.Message5.Text"].Value)
                .Must(IsValidValue).WithMessage(_localization["Validators.County.Message6.Text"].Value)
                .InclusiveBetween(-90.0, 90.0).WithMessage(_localization["Validators.County.Message7.Text"].Value);

            RuleFor(x => x.County.Longitude)
                .NotNull().WithMessage(_localization["Validators.County.Message8.Text"].Value)
                .Must(IsValidValue).WithMessage(_localization["Validators.County.Message9.Text"].Value)
                .InclusiveBetween(-180.0, 180.0).WithMessage(_localization["Validators.County.Message10.Text"].Value);

            RuleFor(x => x.County.CityId)
                .Must(IsCitySelected).WithMessage(_localization["Validators.County.Message11.Text"].Value);
        }

        private bool IsValidValue(double value)
        {
            return !value.Equals(default(Double));
        }

        private bool IsCitySelected(int cityId)
        {
            if (cityId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
