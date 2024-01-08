using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class VisitedCountyFakeDataViewModelValidator : AbstractValidator<FakeDataViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public VisitedCountyFakeDataViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.UserAmount)
                .NotNull().WithMessage(_localization["Validators.FakeData.Message1.Text"].Value)
                .ExclusiveBetween(0, 100).WithMessage(_localization["Validators.FakeData.Message2.Text"].Value);

            RuleFor(x => x.VisitedCountyAmount)
                .NotNull().WithMessage(_localization["Validators.FakeData.Message1.Text"].Value)
                .ExclusiveBetween(0, 100).WithMessage(_localization["Validators.FakeData.Message2.Text"].Value);
        }
    }
}
