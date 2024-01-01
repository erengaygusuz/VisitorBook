using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class SubRegionViewModelValidator : AbstractValidator<SubRegionViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public SubRegionViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.SubRegion.Name)
                .NotNull().WithMessage(_localization["Validators.SubRegion.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.SubRegion.Message2.Text"].Value)
                .MinimumLength(2).WithMessage(_localization["Validators.SubRegion.Message3.Text"].Value)
                .MaximumLength(50).WithMessage(_localization["Validators.SubRegion.Message4.Text"].Value);

            RuleFor(x => x.SubRegion.RegionId)
              .Must(IsRegionSelected).WithMessage(_localization["Validators.SubRegion.Message5.Text"].Value);
        }

        private bool IsRegionSelected(int regionId)
        {
            if (regionId != 0)
            {
                return true;
            }

            return false;
        }
    }
}
