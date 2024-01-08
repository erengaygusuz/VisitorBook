using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class VisitedCountyViewModelValidator : AbstractValidator<VisitedCountyViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public VisitedCountyViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.VisitedCounty.UserId)
               .Must(IsUserSelected).WithMessage(_localization["Validators.VisitedCounty.Message1.Text"].Value);

            RuleFor(x => x.VisitedCounty.CityId)
               .Must(IsCitySelected).WithMessage(_localization["Validators.VisitedCounty.Message2.Text"].Value);

            RuleFor(x => x.VisitedCounty.CountyId)
               .Must(IsCountySelected).WithMessage(_localization["Validators.VisitedCounty.Message3.Text"].Value);

            RuleFor(x => x.VisitedCounty.VisitDate)
               .NotNull().WithMessage(_localization["Validators.VisitedCounty.Message4.Text"].Value)
               .Must(IsValidDate).WithMessage(_localization["Validators.VisitedCounty.Message5.Text"].Value);
        }

        private bool IsValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        private bool IsUserSelected(int userId)
        {
            if (userId != 0)
            {
                return true;
            }

            return false;
        }

        private bool IsCitySelected(int cityId)
        {
            if (cityId != 0)
            {
                return true;
            }

            return false;
        }

        private bool IsCountySelected(int countyId)
        {
            if (countyId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
