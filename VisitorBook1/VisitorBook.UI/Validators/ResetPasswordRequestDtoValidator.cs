using FluentValidation;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.UI.Languages;
namespace VisitorBook.UI.Validators
{
    public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
    {
        private readonly IStringLocalizer<Language> _localization;
        public ResetPasswordRequestDtoValidator(IStringLocalizer<Language> localization)
        {
            _localization = localization; 

            RuleFor(x => x.Password)
                .NotNull().WithMessage(_localization["Validators.ResetPassword.Message1.Text"].Value);

            RuleFor(x => x.PasswordConfirm)
                .NotNull().WithMessage(_localization["Validators.ResetPassword.Message2.Text"].Value)
                .Equal(x => x.Password).WithMessage(_localization["Validators.ResetPassword.Message3.Text"].Value);
        }
    }
}
