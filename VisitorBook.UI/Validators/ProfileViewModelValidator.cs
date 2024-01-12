using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Validators
{
    public class ProfileViewModelValidator : AbstractValidator<ProfileViewModel>
    {
        private readonly IStringLocalizer<Language> _localization;

        public ProfileViewModelValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization;

            When(x => x.UserGeneralInfo != null, () => {
                RuleFor(x => x.UserGeneralInfo.Name)
                .NotNull().WithMessage(_localization["Validators.Profile.Message1.Text"].Value)
                .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Profile.Message2.Text"].Value)
                .MinimumLength(3).WithMessage(_localization["Validators.Profile.Message3.Text"].Value)
                .MaximumLength(100).WithMessage(_localization["Validators.Profile.Message4.Text"].Value);

                RuleFor(x => x.UserGeneralInfo.Surname)
                    .NotNull().WithMessage(_localization["Validators.Profile.Message5.Text"].Value)
                    .Matches("^((?![0-9]).)*$").WithMessage(_localization["Validators.Profile.Message6.Text"].Value)
                    .MinimumLength(3).WithMessage(_localization["Validators.Profile.Message7.Text"].Value)
                    .MaximumLength(100).WithMessage(_localization["Validators.Profile.Message8.Text"].Value);

                RuleFor(x => x.UserGeneralInfo.BirthDate)
                    .NotNull().WithMessage(_localization["Validators.Profile.Message9.Text"].Value)
                    .Must(IsValidDate).WithMessage(_localization["Validators.Profile.Message10.Text"].Value);

                RuleFor(x => x.UserGeneralInfo.Gender)
                    .NotNull().WithMessage(_localization["Validators.Profile.Message11.Text"].Value);

                RuleFor(x => x.UserGeneralInfo.PhoneNumber)
                    .NotNull().WithMessage(_localization["Validators.Profile.Message12.Text"].Value);

                When(x => x.UserGeneralInfo.Picture != null, () =>
                {
                    RuleFor(x => x.UserGeneralInfo.Picture)
                        .Must(x => x.ContentType.Equals("image/jpeg") || x.ContentType.Equals("image/jpg") || x.ContentType.Equals("image/png"))
                        .WithMessage(_localization["Validators.Profile.Message20.Text"].Value);
                });
                    
            });

            When(x => x.UserSecurityInfo != null, () => {
                RuleFor(x => x.UserSecurityInfo.PasswordOld)
                .NotNull().WithMessage(_localization["Validators.Profile.Message13.Text"].Value)
                .MinimumLength(5).WithMessage(_localization["Validators.Profile.Message14.Text"].Value);

                RuleFor(x => x.UserSecurityInfo.PasswordNew)
                    .NotNull().WithMessage(_localization["Validators.Profile.Message15.Text"].Value)
                    .MinimumLength(5).WithMessage(_localization["Validators.Profile.Message16.Text"].Value);

                RuleFor(x => x.UserSecurityInfo.PasswordNewConfirm)
                    .NotNull().WithMessage(_localization["Validators.Profile.Message17.Text"].Value)
                    .Equal(x => x.UserSecurityInfo.PasswordNew).WithMessage(_localization["Validators.Profile.Message18.Text"].Value)
                    .MinimumLength(5).WithMessage(_localization["Validators.Profile.Message19.Text"].Value);
            });
        }

        private bool IsValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
