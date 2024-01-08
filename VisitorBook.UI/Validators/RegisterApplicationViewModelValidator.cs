using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class RegisterApplicationViewModelValidator : AbstractValidator<RegisterApplicationViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public RegisterApplicationViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            RuleFor(x => x.RegisterApplication.Status)
                .NotNull().WithMessage(_localization["Validators.RegisterApplication.Message21.Text"].Value);
        }
    }
}
